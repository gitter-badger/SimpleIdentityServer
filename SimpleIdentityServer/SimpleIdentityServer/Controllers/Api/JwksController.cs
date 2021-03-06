﻿using System.Web.Http;

using SimpleIdentityServer.Core.Api.Jwks;
using SimpleIdentityServer.Core.Jwt.Signature;

namespace SimpleIdentityServer.Api.Controllers.Api
{
    public class JwksController : ApiController
    {
        private readonly IJwksActions _jwksActions;

        public JwksController(IJwksActions jwksActions)
        {
            _jwksActions = jwksActions;
        }

        public JsonWebKeySet Get()
        {
            var jsonWebKeySet = _jwksActions.GetJwks();
            return jsonWebKeySet;
        }
    }
}
