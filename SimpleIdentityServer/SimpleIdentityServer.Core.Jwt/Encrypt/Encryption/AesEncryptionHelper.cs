﻿using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using SimpleIdentityServer.Core.Jwt.Encrypt.Algorithms;

namespace SimpleIdentityServer.Core.Jwt.Encrypt.Encryption
{
    public interface IAesEncryptionHelper
    {
        byte[] GenerateContentEncryptionKey(int keySize);

        byte[] EncryptContentEncryptionKey(
            byte[] contentEncryptionKey,
            JweAlg alg,
            JsonWebKey jsonWebKey);

        byte[] DecryptContentEncryptionKey(
            byte[] encryptedContentEncryptionKey,
            JweAlg alg,
            JsonWebKey jsonWebKey);

        byte[] EncryptWithAesAlgorithm(
            string toEncrypt,
            byte[] key,
            byte[] iv);

        string DecryptWithAesAlgorithm(
            byte[] cipherText,
            byte[] key,
            byte[] iv);
    }

    public class AesEncryptionHelper : IAesEncryptionHelper
    {
        private Dictionary<JweAlg, IAlgorithm> _mappingJweAlgToAlgorithms = new Dictionary<JweAlg, IAlgorithm>
        {
            {
                JweAlg.RSA1_5, new RsaAlgorithm(false)
            },
            {
                JweAlg.RSA_OAEP, new RsaAlgorithm(true)
            }
        };

        public byte[] GenerateContentEncryptionKey(int keySize)
        {
            return ByteManipulator.GenerateRandomBytes(keySize);
        }

        public byte[] EncryptContentEncryptionKey(
            byte[] contentEncryptionKey,
            JweAlg alg,
            JsonWebKey jsonWebKey)
        {
            var algorithm = _mappingJweAlgToAlgorithms[alg];
            return algorithm.Encrypt(
                contentEncryptionKey, 
                jsonWebKey);
        }

        public byte[] DecryptContentEncryptionKey(
            byte[] encryptedContentEncryptionKey,
            JweAlg alg,
            JsonWebKey jsonWebKey)
        {
            var algorithm = _mappingJweAlgToAlgorithms[alg];
            return algorithm.Decrypt(
                encryptedContentEncryptionKey,
                jsonWebKey);
        }

        public byte[] EncryptWithAesAlgorithm(
            string toEncrypt,
            byte[] key,
            byte[] iv)
        {
            byte[] result;
            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(toEncrypt);
                            }

                            result = msEncrypt.ToArray();
                        }
                    }
                }
            }

            return result;
        }

        public string DecryptWithAesAlgorithm(byte[] cipherText, byte[] key, byte[] iv)
        {
            string result;
            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
