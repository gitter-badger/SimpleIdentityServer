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
using System.Collections.Generic;
using System.Security.Claims;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Core.Results;
using SimpleIdentityServer.Core.JwtToken;
using SimpleIdentityServer.Logging;

namespace SimpleIdentityServer.Core.Common
{
    public interface IGenerateAuthorizationResponse
    {
        void Execute(
            ActionResult actionResult,
            AuthorizationParameter authorizationParameter,
            ClaimsPrincipal claimsPrincipal);
    }

    public class GenerateAuthorizationResponse : IGenerateAuthorizationResponse
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;

        private readonly IParameterParserHelper _parameterParserHelper;

        private readonly IJwtGenerator _jwtGenerator;

        private readonly IGrantedTokenGeneratorHelper _grantedTokenGeneratorHelper;

        private readonly IGrantedTokenRepository _grantedTokenRepository;

        private readonly IConsentHelper _consentHelper;

        private readonly ISimpleIdentityServerEventSource _simpleIdentityServerEventSource;

        public GenerateAuthorizationResponse(
            IAuthorizationCodeRepository authorizationCodeRepository,
            IParameterParserHelper parameterParserHelper,
            IJwtGenerator jwtGenerator,
            IGrantedTokenGeneratorHelper grantedTokenGeneratorHelper,
            IGrantedTokenRepository grantedTokenRepository,
            IConsentHelper consentHelper,
            ISimpleIdentityServerEventSource simpleIdentityServerEventSource)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
            _parameterParserHelper = parameterParserHelper;
            _jwtGenerator = jwtGenerator;
            _grantedTokenGeneratorHelper = grantedTokenGeneratorHelper;
            _grantedTokenRepository = grantedTokenRepository;
            _consentHelper = consentHelper;
            _simpleIdentityServerEventSource = simpleIdentityServerEventSource;
        }

        #region Public methods

        public void Execute(
            ActionResult actionResult, 
            AuthorizationParameter authorizationParameter,
            ClaimsPrincipal claimsPrincipal)
        {
            if (authorizationParameter == null)
            {
                throw new ArgumentNullException("cannot generate the authorization response because the authorization parameter is null");    
            }

            if (actionResult == null || actionResult.RedirectInstruction == null)
            {
                throw new ArgumentNullException("cannot generate the authorization response because the action result is null");
            }

            _simpleIdentityServerEventSource.StartGeneratingAuthorizationResponseToClient(authorizationParameter.ClientId,
                authorizationParameter.ResponseType);

            var responses = _parameterParserHelper.ParseResponseType(authorizationParameter.ResponseType);
            var idTokenPayload = GenerateIdTokenPayload(claimsPrincipal, authorizationParameter);
            var userInformationPayload = GenerateUserInformationPayload(claimsPrincipal, authorizationParameter);

            if (responses.Contains(ResponseType.id_token))
            {
                var idToken = GenerateIdToken(idTokenPayload, authorizationParameter);
                actionResult.RedirectInstruction.AddParameter(Constants.StandardAuthorizationResponseNames.IdTokenName, idToken);
            }

            if (responses.Contains(ResponseType.token))
            {
                var allowedTokenScopes = string.Empty;
                if (!string.IsNullOrWhiteSpace(authorizationParameter.Scope))
                {
                    allowedTokenScopes = string.Join(" ", _parameterParserHelper.ParseScopeParameters(authorizationParameter.Scope));
                }

                // Check if an access token has already been generated and can be reused for that :
                // We assumed that an access token is unique for a specific client id, user information & id token payload & for certain scopes
                var grantedToken = _grantedTokenRepository.GetToken(
                    allowedTokenScopes,
                    authorizationParameter.ClientId,
                    idTokenPayload,
                    userInformationPayload);
                if (grantedToken == null)
                {
                    grantedToken = _grantedTokenGeneratorHelper.GenerateToken(
                        authorizationParameter.ClientId,
                        allowedTokenScopes,
                        userInformationPayload,
                        idTokenPayload);

                    _grantedTokenRepository.Insert(grantedToken);
                    _simpleIdentityServerEventSource.GrantAccessToClient(authorizationParameter.ClientId,
                        grantedToken.AccessToken,
                        allowedTokenScopes);
                }

                actionResult.RedirectInstruction.AddParameter(Constants.StandardAuthorizationResponseNames.AccessTokenName, grantedToken.AccessToken);
            }

            if (responses.Contains(ResponseType.code))
            {
                var assignedConsent = _consentHelper.GetConsentConfirmedByResourceOwner(claimsPrincipal.GetSubject(), authorizationParameter);
                if (assignedConsent != null)
                {
                    // Insert a temporary authorization code 
                    // It will be used later to retrieve tha id_token or an access token.
                    var authorizationCode = new AuthorizationCode
                    {
                        Code = Guid.NewGuid().ToString(),
                        RedirectUri = authorizationParameter.RedirectUrl,
                        CreateDateTime = DateTime.UtcNow,
                        ClientId = authorizationParameter.ClientId,
                        Scopes = authorizationParameter.Scope,
                        IdTokenPayload = idTokenPayload,
                        UserInfoPayLoad = userInformationPayload
                    };

                    _authorizationCodeRepository.AddAuthorizationCode(authorizationCode);
                    _simpleIdentityServerEventSource.GrantAuthorizationCodeToClient(authorizationParameter.ClientId,
                        authorizationCode.Code,
                        authorizationParameter.Scope);
                    actionResult.RedirectInstruction.AddParameter(Constants.StandardAuthorizationResponseNames.AuthorizationCodeName, authorizationCode.Code);
                }
            }

            if (!string.IsNullOrWhiteSpace(authorizationParameter.State))
            {
                actionResult.RedirectInstruction.AddParameter(Constants.StandardAuthorizationResponseNames.StateName, authorizationParameter.State);
            }

            if (authorizationParameter.ResponseMode == ResponseMode.form_post)
            {
                actionResult.Type = TypeActionResult.RedirectToAction;
                actionResult.RedirectInstruction.Action = IdentityServerEndPoints.FormIndex;
                actionResult.RedirectInstruction.AddParameter("redirect_uri", authorizationParameter.RedirectUrl);
            }

            _simpleIdentityServerEventSource.EndGeneratingAuthorizationResponseToClient(authorizationParameter.ClientId,
               actionResult.RedirectInstruction.Parameters.SerializeWithJavascript());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generate the JWS payload for identity token.
        /// If at least one claim is defined then returns the filtered result
        /// Otherwise returns the default payload based on the scopes.
        /// </summary>
        /// <param name="jwsPayload"></param>
        /// <param name="authorizationParameter"></param>
        /// <returns></returns>
        private string GenerateIdToken(
            JwsPayload jwsPayload,
            AuthorizationParameter authorizationParameter)
        {
            var idToken = _jwtGenerator.Sign(jwsPayload, authorizationParameter.ClientId);
            return _jwtGenerator.Encrypt(idToken, authorizationParameter.ClientId);
        }

        private JwsPayload GenerateIdTokenPayload(
            ClaimsPrincipal claimsPrincipal,
            AuthorizationParameter authorizationParameter)
        {
            JwsPayload jwsPayload;
            if (authorizationParameter.Claims != null &&
                authorizationParameter.Claims.IsAnyIdentityTokenClaimParameter())
            {
                jwsPayload = _jwtGenerator.GenerateFilteredIdTokenPayload(
                    claimsPrincipal,
                    authorizationParameter,
                    authorizationParameter.Claims.IdToken);
            }
            else
            {
                jwsPayload = _jwtGenerator.GenerateIdTokenPayloadForScopes(claimsPrincipal, authorizationParameter);
            }

            return jwsPayload;
        }

        /// <summary>
        /// Generate the JWS payload for user information endpoint.
        /// If at least one claim is defined then returns the filtered result
        /// Otherwise returns the default payload based on the scopes.
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="authorizationParameter"></param>
        /// <returns></returns>
        private JwsPayload GenerateUserInformationPayload(
            ClaimsPrincipal claimsPrincipal,
            AuthorizationParameter authorizationParameter)
        {
            JwsPayload jwsPayload;
            if (authorizationParameter.Claims != null &&
                authorizationParameter.Claims.IsAnyUserInfoClaimParameter())
            {
                jwsPayload = _jwtGenerator.GenerateFilteredUserInfoPayload(
                    authorizationParameter.Claims.UserInfo,
                    claimsPrincipal,
                    authorizationParameter);
            }
            else
            {
                jwsPayload = _jwtGenerator.GenerateUserInfoPayloadForScope(claimsPrincipal, authorizationParameter);
            }

            return jwsPayload;
        }

        #endregion
    }
}
