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
using System.Collections.Generic;
using System.Security.Claims;
using SimpleIdentityServer.Core.WebSite.Consent.Actions;
using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.Results;

namespace SimpleIdentityServer.Core.WebSite.Consent
{
    public interface IConsentActions
    {
        ActionResult DisplayConsent(
            AuthorizationParameter authorizationParameter,
            out Client client,
            out List<Scope> allowedScopes,
            out List<string> allowedClaims);

        ActionResult ConfirmConsent(
            AuthorizationParameter authorizationParameter,
            ClaimsPrincipal claimsPrincipal);
    }

    public class ConsentActions : IConsentActions
    {
        private readonly IDisplayConsentAction _displayConsentAction;

        private readonly IConfirmConsentAction _confirmConsentAction;

        public ConsentActions(
            IDisplayConsentAction displayConsentAction,
            IConfirmConsentAction confirmConsentAction)
        {
            _displayConsentAction = displayConsentAction;
            _confirmConsentAction = confirmConsentAction;
        }

        public ActionResult DisplayConsent(
            AuthorizationParameter authorizationParameter,
            out Client client,
            out List<Scope> allowedScopes,
            out List<string> allowedClaims)
        {
            return _displayConsentAction.Execute(authorizationParameter,
                out client,
                out allowedScopes,
                out allowedClaims);
        }

        public ActionResult ConfirmConsent(
            AuthorizationParameter authorizationParameter,
            ClaimsPrincipal claimsPrincipal)
        {
            return _confirmConsentAction.Execute(authorizationParameter,
                claimsPrincipal);
        }
    }
}
