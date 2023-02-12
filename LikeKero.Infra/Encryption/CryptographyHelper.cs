using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LikeKero.Infra.Encryption
{
    public class RSAKeys
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
    public class CryptographyHelper
    {
        ///https://stackoverflow.com/questions/59243960/issue-decrypting-rsa-public-key-encrypted-data-by-js-in-c-sharp

        ///Source: https://stackoverflow.com/questions/17128038/c-sharp-rsa-encryption-decryption-with-transmission
        public static RSAKeys GenerateKeys()
        {
            //CSP with a new 2048 bit rsa key pair
            var csp = new RSACryptoServiceProvider(2048);

            //Private key
            var privKey = csp.ExportParameters(true);

            //Public key
            var pubKey = csp.ExportParameters(false);

            string privKeyString = string.Empty;

            var sw = new System.IO.StringWriter();
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, privKey);
            privKeyString = sw.ToString();

            //This will give public key in the following format which is required by the JS library
            //-----BEGIN PUBLIC KEY-----
            //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            //-----END PUBLIC KEY-----
            string publicKeyBase64 = ConvertPublicKeyForJS(pubKey);

            //Will be saved in sesssion for later use during decryption
            string privateKeyBase64 = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(privKeyString));

            var rsaKeysTypes = new RSAKeys();
            rsaKeysTypes.PublicKey = publicKeyBase64;
            rsaKeysTypes.PrivateKey = privateKeyBase64;

            return rsaKeysTypes;
            //Save in session/temp location
            //Return publicKeyBase64 to JS
        }

        /// <summary>
        /// Source: https://stackoverflow.com/questions/28406888/c-sharp-rsa-public-key-output-not-correct/28407693#28407693
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string ConvertPublicKeyForJS(RSAParameters publicKey)
        {
            string output = string.Empty;

            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, publicKey.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, publicKey.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);

                StringBuilder sb = new StringBuilder();
                sb.Append(@"-----BEGIN PUBLIC KEY-----");
                sb.Append(base64);
                sb.Append(@"-----END PUBLIC KEY-----");

                output = sb.ToString();
            }

            return output;
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        /// Source: https://stackoverflow.com/questions/27727024/encrypt-in-javascript-will-not-decrypt-in-c-sharp

        public static string DecryptValue(string cypherText, string privKeyBase64)
        {
            try
            {
                string plainTextData = string.Empty;

                string privKeyString = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(privKeyBase64));

                var sr = new System.IO.StringReader(privKeyString);
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                var privKey = (RSAParameters)xs.Deserialize(sr);

                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(privKey);

                var bytesCypherText = Convert.FromBase64String(cypherText);

                IEnumerable<byte> output = new List<byte>();
                for (var i = 0; i < bytesCypherText.Length; i = i + bytesCypherText.Length)
                {
                    var length = Math.Max(bytesCypherText.Length - i, 128);
                    var block = new byte[length];
                    Buffer.BlockCopy(bytesCypherText, i, block, 0, length);
                    var chunk = csp.Decrypt(block, false);
                    output = output.Concat(chunk);
                }
                return Encoding.UTF8.GetString(output.ToArray());
            }
            catch
            {
                return cypherText;
            }

        }
    }
}
