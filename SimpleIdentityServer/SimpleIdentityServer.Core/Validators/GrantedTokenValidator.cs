﻿#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Core.Errors;

namespace SimpleIdentityServer.Core.Validators
{
    public interface IGrantedTokenValidator
    {
        bool CheckAccessToken(
            string accessToken, 
            out string messageErrorCode,
            out string messageErrorDescription);
    }

    public class GrantedTokenValidator : IGrantedTokenValidator
    {
        private readonly IGrantedTokenRepository _grantedTokenRepository;

        public GrantedTokenValidator(IGrantedTokenRepository grantedTokenRepository)
        {
            _grantedTokenRepository = grantedTokenRepository;
        }

        public bool CheckAccessToken(
            string accessToken,
            out string messageErrorCode,
            out string messageErrorDescription)
        {
            messageErrorCode = string.Empty;
            messageErrorDescription = string.Empty;
            var grantedToken = _grantedTokenRepository.GetToken(accessToken);
            if (grantedToken == null)
            {
                messageErrorCode = ErrorCodes.InvalidToken;
                messageErrorDescription = ErrorDescriptions.TheAccessTokenIsNotValid;
                return false;
            }

            var expirationDateTime = grantedToken.CreateDateTime.AddSeconds(grantedToken.ExpiresIn);
            var tokenIsExpired = DateTime.UtcNow > expirationDateTime;
            if (tokenIsExpired)
            {
                messageErrorCode = ErrorCodes.InvalidToken;
                messageErrorDescription = ErrorDescriptions.TheAccessTokenIsExpired;
                return false;
            }
                                  
            return true;
        }
    }
}
