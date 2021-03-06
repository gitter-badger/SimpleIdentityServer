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
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.Core.Models;
using System.Text;

namespace SimpleIdentityServer.Core.Helpers
{
    public interface IGrantedTokenGeneratorHelper
    {
        GrantedToken GenerateToken(
            string clientId,
            string scope,
            JwsPayload userInformationPayload = null,
            JwsPayload idTokenPayload = null);
    }

    // TODO : rename to granted token factory
    public class GrantedTokenGeneratorHelper : IGrantedTokenGeneratorHelper
    {
        public GrantedToken GenerateToken(
            string clientId,
            string scope,
            JwsPayload userInformationPayload = null,
            JwsPayload idTokenPayload = null)
        {
            var accessTokenId = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            var refreshTokenId = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            return new GrantedToken
            {
                AccessToken = Convert.ToBase64String(accessTokenId),
                RefreshToken = Convert.ToBase64String(refreshTokenId),
                ExpiresIn = 3600,
                TokenType = Constants.StandardTokenTypes.Bearer,
                CreateDateTime = DateTime.UtcNow,
                // IDS
                Scope = scope,
                UserInfoPayLoad = userInformationPayload,
                IdTokenPayLoad = idTokenPayload,
                ClientId = clientId
            };
        }
    }
}
