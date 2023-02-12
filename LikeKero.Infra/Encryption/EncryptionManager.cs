using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LikeKero.Infra.Encryption
{
    public class EncryptionManager
    {
        #region Variables
        // define the triple des provider
        private static TripleDESCryptoServiceProvider m_des =
                 new TripleDESCryptoServiceProvider();

        // define the string handler
        private static UTF8Encoding m_utf8 = new UTF8Encoding();

        // define the local property arrays
        private static byte[] yM_key ={1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
              13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
        private static byte[] yM_iv = { 8, 7, 6, 5, 4, 3, 2, 1 };
        #endregion

        #region Constructor
        /// <summary>
        /// EncryptionManager
        /// </summary>
        public EncryptionManager()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Encryption for byte
        /// </summary>
        /// <param name="pInput"></param>
        /// <returns>returns encrypted byte object</returns>
        public byte[] Encrypt(byte[] pInput)
        {
            return Transform(pInput,
                   m_des.CreateEncryptor(yM_key, yM_iv));
        }

        /// <summary>
        /// Decryption for byte
        /// </summary>
        /// <param name="pInput"></param>
        /// <returns>returns decrypted byte array object</returns>
        public byte[] Decrypt(byte[] pInput)
        {
            return Transform(pInput,
                   m_des.CreateDecryptor(yM_key, yM_iv));
        }

        /// <summary>
        /// Encryption for string
        /// </summary>
        /// <param name="pText"></param>
        /// <returns>returns encrypted string</returns>
        public static string Encrypt(string pText)
        {
            byte[] yInput = m_utf8.GetBytes(pText);
            byte[] yOutput = Transform(yInput,
                            m_des.CreateEncryptor(yM_key, yM_iv));
            return Convert.ToBase64String(yOutput);
        }

        /// <summary>
        /// Decryption for string
        /// </summary>
        /// <param name="pText"></param>
        /// <returns>returns decrypted string object</returns>
        public static string Decrypt(string pText)
        {
            //Check for Spaces
            pText = ValidationManager.CheckEncryptedQueryString(pText);
            byte[] yInput = Convert.FromBase64String(pText);
            byte[] yOutput = Transform(yInput,
                            m_des.CreateDecryptor(yM_key, yM_iv));
            return m_utf8.GetString(yOutput);
        }

        /// <summary>
        /// Transform byte to string
        /// </summary>
        /// <param name="pInput"></param>
        /// <param name="pCryptoTransform"></param>
        /// <returns>transformed array of byte</returns>
        private static byte[] Transform(byte[] pInput,
                       ICryptoTransform pCryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream,
                         pCryptoTransform, CryptoStreamMode.Write);
            // transform the bytes as requested
            cryptStream.Write(pInput, 0, pInput.Length);
            cryptStream.FlushFinalBlock();
            // Read the memory stream and
            // convert it back into byte array
            memStream.Position = 0;
            byte[] yResult = memStream.ToArray();
            // close and release the streams
            memStream.Close();
            cryptStream.Close();
            // hand back the encrypted buffer
            return yResult;
        }


        /// <summary>
        /// take any string and encrypt it using MD5 then
        /// return the encrypted data 
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        public static string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }


        /// <summary>
        /// encrypt input text using MD5 and compare it with
        /// the stored encrypted text
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">the encrypted text
        ///         stored on file or database ... etc</param>
        /// <returns>true or false depending on input validation</returns>
        public static bool ValidateMD5HashData(string inputData, string storedHashData)
        {
            //hash input text and save it string variable
            string getHashInputData = GetMD5HashData(inputData);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion



        public static string encryptsms(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decryptsms(string cipherText)
        {
            try
            {
                string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch { }
            return cipherText;
        }
    }
}
