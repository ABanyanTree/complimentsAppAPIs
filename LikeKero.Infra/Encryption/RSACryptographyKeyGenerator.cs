using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LikeKero.Infra.Encryption
{
    public enum RSAKeySize
    {
        Key512 = 512,
        Key1024 = 1024,
        Key2048 = 2048,
        Key4096 = 4096
    }

    public class RSAKeysTypes
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public class RSACryptographyKeyGenerator
    {
        public RSAKeysTypes GenerateKeys(RSAKeySize rsaKeySize)
        {
            int keySize = (int)rsaKeySize;
            if (keySize % 2 != 0 || keySize < 512)
                throw new Exception("Key should be multiple of two and greater than 512.");

            var rsaKeysTypes = new RSAKeysTypes();

            using (var provider = new RSACryptoServiceProvider(keySize))
            {

                var publicKey = ToXmlFile(provider, false);
                var privateKey = ToXmlFile(provider, true);

                var publicKeyWithSize = IncludeKeyInEncryptionString(publicKey, keySize);
                var privateKeyWithSize = IncludeKeyInEncryptionString(privateKey, keySize);

                rsaKeysTypes.PublicKey = publicKeyWithSize;
                rsaKeysTypes.PrivateKey = privateKeyWithSize;
            }

            return rsaKeysTypes;
        }

        private string IncludeKeyInEncryptionString(string publicKey, int keySize)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(keySize.ToString() + "!" + publicKey));
        }

        public string ToXmlFile(RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);
            string data =
                string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);

            return data;
        }
    }
}
