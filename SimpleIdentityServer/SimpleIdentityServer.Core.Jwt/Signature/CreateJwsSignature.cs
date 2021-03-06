﻿using SimpleIdentityServer.Core.Common.Extensions;

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SimpleIdentityServer.Core.Jwt.Signature
{
    public interface ICreateJwsSignature
    {
        string SignWithRsa(
            JwsAlg algorithm,
            string serializedKeys,
            string combinedJwsNotSigned);

        bool VerifyWithRsa(
            JwsAlg algorithm,
            string serializedKeys,
            string input,
            byte[] signature);
    }

    public class CreateJwsSignature : ICreateJwsSignature
    {
        private readonly Dictionary<JwsAlg, string> _mappingJwsAlgorithmToRsaHashingAlgorithms = new Dictionary<JwsAlg, string>
        {
            {
                JwsAlg.RS256, "SHA256"
            },
            {
                JwsAlg.RS384, "SHA384"
            },
            {
                JwsAlg.RS512, "SHA512"
            }
        }; 

        public string SignWithRsa(
            JwsAlg algorithm, 
            string serializedKeys,
            string combinedJwsNotSigned)
        {
            if (!_mappingJwsAlgorithmToRsaHashingAlgorithms.ContainsKey(algorithm))
            {
                return null;
            }

            var hashMethod = _mappingJwsAlgorithmToRsaHashingAlgorithms[algorithm];
            using (var rsa = new RSACryptoServiceProvider())
            {
                var bytesToBeSigned = ASCIIEncoding.ASCII.GetBytes(combinedJwsNotSigned);
                rsa.FromXmlString(serializedKeys);
                var byteToBeConverted = rsa.SignData(bytesToBeSigned, hashMethod);
                return byteToBeConverted.Base64EncodeBytes();
            }
        }

        public string SignWithEllipseCurve(JwsAlg algorithm)
        {
            return string.Empty;
        }

        public bool VerifyWithRsa(
            JwsAlg algorithm,
            string serializedKeys,
            string input,
            byte[] signature)
        {
            var plainBytes = ASCIIEncoding.ASCII.GetBytes(input);
            var hashMethod = _mappingJwsAlgorithmToRsaHashingAlgorithms[algorithm];
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(serializedKeys);
                return rsa.VerifyData(plainBytes, hashMethod, signature);
            }
        }
    }
}
