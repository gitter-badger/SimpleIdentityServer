﻿using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Unity;

using SimpleIdentityServer.Api.Configuration;
using SimpleIdentityServer.Api.Parsers;
using SimpleIdentityServer.Core.Configuration;
using SimpleIdentityServer.Core.Protector;
using SimpleIdentityServer.Core.Services;
using SimpleIdentityServer.Logging;

namespace SimpleIdentityServer.Api
{
    public static class UnityConfig
    {
        public static UnityContainer Create(ISimpleIdentityServerEventSource simpleIdentityServerEventSource)
        {
            var container = new UnityContainer();

            container.RegisterType<ICacheManager, CacheManager>();
            container.RegisterType<ICertificateStore, CertificateStore>();
            container.RegisterType<IResourceOwnerService, InMemoryUserService>();
            container.RegisterType<IRedirectInstructionParser, RedirectInstructionParser>();
            container.RegisterType<IActionResultParser, ActionResultParser>();
            container.RegisterType<ISimpleIdentityServerConfigurator, ConcreteSimpleIdentityServerConfigurator>();

            container.RegisterInstance(simpleIdentityServerEventSource);

            ModuleLoader.LoadContainer(container);

            return container;
        }
    }
}