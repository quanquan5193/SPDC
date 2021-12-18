﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Common
{
    public static class EncryptUtilities
    {

        public static string ToBase64(string text)
        {
            if (text == null || text.Length == 0)
            {
                return string.Empty;
            }

            var data = System.Text.Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(data);
        }

        public static string DecodeBase64(string encodeString)
        {
            byte[] data = Convert.FromBase64String(encodeString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public static string GetEncryptedString(byte[] encryptedByte)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < encryptedByte.Length; i++)
            {
                builder.Append(encryptedByte[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static bool CompareTwoByteArray(byte[] arr1, byte[] arr2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(arr1, arr2);
        }

        public static byte[] GetKeyEncryptMobileNumber()
        {
            return Convert.FromBase64CharArray(StaticConfig.KeyEncryptMobile.ToCharArray(), 0, StaticConfig.KeyEncryptMobile.Length);
        }

        public static byte[] GetVectorEncryptMobileNumber()
        {
            return Convert.FromBase64CharArray(StaticConfig.VectorEncryptMobile.ToCharArray(), 0, StaticConfig.VectorEncryptMobile.Length);
        }

        public static byte[] EncryptAes256(string raw)
        {
            AESModel aesModel = new AESModel(GetKeyEncryptMobileNumber(), GetVectorEncryptMobileNumber());

            byte[] encrypted;

            ICryptoTransform encryptor = aesModel.aes.CreateEncryptor(aesModel.Key, aesModel.Vector);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    // Create StreamWriter and write data to a stream    
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(raw);
                    encrypted = ms.ToArray();
                }
            }

            return encrypted;
        }

        public static string DecryptAes256(byte[] cipherText)
        {
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Padding = PaddingMode.PKCS7;
                aes.BlockSize = 128;
                ICryptoTransform decryptor = aes.CreateDecryptor(GetKeyEncryptMobileNumber(), GetVectorEncryptMobileNumber());
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        //public static byte[] EncryptAes256(AESModel aesModel, string raw)
        //{
        //    byte[] encrypted;

        //    ICryptoTransform encryptor = aesModel.aes.CreateEncryptor(aesModel.Key, aesModel.Vector);

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //        {
        //            // Create StreamWriter and write data to a stream    
        //            using (StreamWriter sw = new StreamWriter(cs))
        //                sw.Write(raw);
        //            encrypted = ms.ToArray();
        //        }
        //    }

        //    return encrypted;
        //}

        //public static string DecryptAes256(byte[] cipherText, byte[] Key, byte[] IV)
        //{
        //    string plaintext = null;
        //    using (AesManaged aes = new AesManaged())
        //    {
        //        aes.KeySize = 256;
        //        aes.Padding = PaddingMode.PKCS7;
        //        aes.BlockSize = 128;
        //        ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
        //        using (MemoryStream ms = new MemoryStream(cipherText))
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader reader = new StreamReader(cs))
        //                    plaintext = reader.ReadToEnd();
        //            }
        //        }
        //    }
        //    return plaintext;
        //}

        #region Decrypt AES from client

        public static string OpenSSLEncrypt(string plainText, string passphrase = StaticConfig.PublichKey)
        {
            // generate salt
            byte[] key, iv;
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);
            DeriveKeyAndIV(passphrase, salt, out key, out iv);
            // encrypt bytes
            byte[] encryptedBytes = EncryptStringToBytesAes(plainText, key, iv);
            // add salt as first 8 bytes
            byte[] encryptedBytesWithSalt = new byte[salt.Length + encryptedBytes.Length + 8];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes("Salted__"), 0, encryptedBytesWithSalt, 0, 8);
            Buffer.BlockCopy(salt, 0, encryptedBytesWithSalt, 8, salt.Length);
            Buffer.BlockCopy(encryptedBytes, 0, encryptedBytesWithSalt, salt.Length + 8, encryptedBytes.Length);
            // base64 encode
            return Convert.ToBase64String(encryptedBytesWithSalt);
        }

        public static string OpenSSLDecrypt(string encrypted, string passphrase = StaticConfig.PublichKey)
        {
            // base 64 decode
            byte[] encryptedBytesWithSalt = Convert.FromBase64String(encrypted);
            // extract salt (first 8 bytes of encrypted)
            byte[] salt = new byte[8];
            byte[] encryptedBytes = new byte[encryptedBytesWithSalt.Length - salt.Length - 8];
            Buffer.BlockCopy(encryptedBytesWithSalt, 8, salt, 0, salt.Length);
            Buffer.BlockCopy(encryptedBytesWithSalt, salt.Length + 8, encryptedBytes, 0, encryptedBytes.Length);
            // get key and iv
            byte[] key, iv;
            DeriveKeyAndIV(passphrase, salt, out key, out iv);
            return DecryptStringFromBytesAes(encryptedBytes, key, iv);
        }

        private static void DeriveKeyAndIV(string passphrase, byte[] salt, out byte[] key, out byte[] iv)
        {
            // generate key and iv
            List<byte> concatenatedHashes = new List<byte>(48);

            byte[] password = Encoding.UTF8.GetBytes(passphrase);
            byte[] currentHash = new byte[0];
            MD5 md5 = MD5.Create();
            bool enoughBytesForKey = false;
            // See http://www.openssl.org/docs/crypto/EVP_BytesToKey.html#KEY_DERIVATION_ALGORITHM
            while (!enoughBytesForKey)
            {
                int preHashLength = currentHash.Length + password.Length + salt.Length;
                byte[] preHash = new byte[preHashLength];

                Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
                Buffer.BlockCopy(password, 0, preHash, currentHash.Length, password.Length);
                Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + password.Length, salt.Length);

                currentHash = md5.ComputeHash(preHash);
                concatenatedHashes.AddRange(currentHash);

                if (concatenatedHashes.Count >= 48)
                    enoughBytesForKey = true;
            }

            key = new byte[32];
            iv = new byte[16];
            concatenatedHashes.CopyTo(0, key, 0, 32);
            concatenatedHashes.CopyTo(32, iv, 0, 16);

            md5.Clear();
            md5 = null;
        }

        static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt;

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush();
                        swEncrypt.Close();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }

        static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                            srDecrypt.Close();
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        #endregion

        /// <summary>
        /// EncryptAes256 raw to string
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static string EncryptAes256ToString(string raw)
        {
            string encryptedString = null;
            if (!string.IsNullOrEmpty(raw))
            {
                var encryptedBytes = EncryptAes256(raw);
                encryptedString = encryptedBytes == null ? null : GetEncryptedString(encryptedBytes);
            }                

            return encryptedString;
        }
    }

    public class AESModel
    {
        public AesManaged aes { get; set; }
        public byte[] Key { get; set; }
        public byte[] Vector { get; set; }

        public AESModel()
        {
            aes = new AesManaged();
            aes.KeySize = 256;
            Key = aes.Key;
            Vector = aes.IV;
        }

        public AESModel(byte[] key, byte[] vector)
        {
            aes = new AesManaged();
            aes.KeySize = 256;
            Key = key;
            Vector = vector;
        }
    }
}
