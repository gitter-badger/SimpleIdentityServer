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
using System.Linq;

using SimpleIdentityServer.Core.Errors;
using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.Models;

namespace SimpleIdentityServer.Core.Validators
{
    public interface IAuthorizationCodeGrantTypeParameterAuthEdpValidator
    {
        void Validate(AuthorizationParameter parameter);
    }

    public sealed class AuthorizationCodeGrantTypeParameterAuthEdpValidator : IAuthorizationCodeGrantTypeParameterAuthEdpValidator
    {
        private readonly IParameterParserHelper _parameterParserHelper;

        private readonly IClientValidator _clientValidator;

        public AuthorizationCodeGrantTypeParameterAuthEdpValidator(
            IParameterParserHelper parameterParserHelper,
            IClientValidator clientValidator)
        {
            _parameterParserHelper = parameterParserHelper;
            _clientValidator = clientValidator;
        }

        public void Validate(AuthorizationParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.Scope))
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.MissingParameter, "scope"),
                    parameter.State);
            }

            if (string.IsNullOrWhiteSpace(parameter.ClientId))
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.MissingParameter, "clientId"),
                    parameter.State);
            }

            if (string.IsNullOrWhiteSpace(parameter.RedirectUrl))
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.MissingParameter, "redirect_uri"),
                    parameter.State);
            }

            if (string.IsNullOrWhiteSpace(parameter.ResponseType))
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.MissingParameter, "response_type"),
                    parameter.State);
            }

            ValidateResponseTypeParameter(parameter.ResponseType, parameter.State);
            ValidatePromptParameter(parameter.Prompt, parameter.State);

            // With this instruction
            // The redirect_uri is considered well-formed according to the RFC-3986
            var redirectUrlIsCorrect = Uri.IsWellFormedUriString(parameter.RedirectUrl, UriKind.Absolute);
            if (!redirectUrlIsCorrect)
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestUriCode,
                    ErrorDescriptions.TheRedirectionUriIsNotWellFormed,
                    parameter.State);
            }

            var client = _clientValidator.ValidateClientExist(parameter.ClientId);
            if (client == null)
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.ClientIsNotValid, parameter.ClientId),
                    parameter.State);
            }

            if (string.IsNullOrWhiteSpace(_clientValidator.ValidateRedirectionUrl(parameter.RedirectUrl, client)))
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    string.Format(ErrorDescriptions.RedirectUrlIsNotValid, parameter.RedirectUrl),
                    parameter.State);
            }
        }

        /// <summary>
        /// Validate the response type parameter.
        /// Returns an exception if at least one response_type parameter is not supported
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="state"></param>
        private void ValidateResponseTypeParameter(
            string responseType, 
            string state)
        {
            if (string.IsNullOrWhiteSpace(responseType))
            {
                return;
            }

            var responseTypeNames = Enum.GetNames(typeof(ResponseType));
            var atLeastOneResonseTypeIsNotSupported = responseType.Split(' ')
                .Any(r => !string.IsNullOrWhiteSpace(r) && !responseTypeNames.Contains(r));
            if (atLeastOneResonseTypeIsNotSupported)
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    ErrorDescriptions.AtLeastOneResponseTypeIsNotSupported,
                    state);
            }
        }

        /// <summary>
        /// Validate the prompt parameter.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="state"></param>
        private void ValidatePromptParameter(
            string prompt,
            string state)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return;
            }

            var promptNames = Enum.GetNames(typeof (PromptParameter));
            var atLeastOnePromptIsNotSupported = prompt.Split(' ')
                .Any(r => !string.IsNullOrWhiteSpace(r) && !promptNames.Contains(r));
            if (atLeastOnePromptIsNotSupported)
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    ErrorDescriptions.AtLeastOnePromptIsNotSupported,
                    state);
            }

            var prompts = _parameterParserHelper.ParsePromptParameters(prompt);
            if (prompts.Contains(PromptParameter.none) &&
                (prompts.Contains(PromptParameter.login) ||
                prompts.Contains(PromptParameter.consent) ||
                prompts.Contains(PromptParameter.select_account)))
            {
                throw new IdentityServerExceptionWithState(
                    ErrorCodes.InvalidRequestCode,
                    ErrorDescriptions.PromptParameterShouldHaveOnlyNoneValue,
                    state
                    );
            }
        }
    }
}
