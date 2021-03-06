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
using SimpleIdentityServer.Core.Errors;
using SimpleIdentityServer.Core.Exceptions;
using SimpleIdentityServer.Core.Helpers;
using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Repositories;

namespace SimpleIdentityServer.Core.Validators
{
    public interface IResourceOwnerValidator
    {
        ResourceOwner ValidateResourceOwnerCredentials(string userName, string password);
    }

    public class ResourceOwnerValidator : IResourceOwnerValidator
    {
        private readonly IResourceOwnerRepository _resourceOwnerRepository;

        private readonly ISecurityHelper _securityHelper;

        public ResourceOwnerValidator(
            IResourceOwnerRepository resourceOwnerRepository,
            ISecurityHelper securityHelper)
        {
            _resourceOwnerRepository = resourceOwnerRepository;
            _securityHelper = securityHelper;
        }

        public ResourceOwner ValidateResourceOwnerCredentials(string userName, string password)
        {
            var hashPassword = _securityHelper.ComputeHash(password);
            var resourceOwner = _resourceOwnerRepository.GetResourceOwnerByCredentials(userName, hashPassword);
            if (resourceOwner == null)
            {
                throw new IdentityServerException(
                    ErrorCodes.InvalidGrant,
                    ErrorDescriptions.ResourceOwnerCredentialsAreNotValid);
            }

            return resourceOwner;
        }
    }
}
