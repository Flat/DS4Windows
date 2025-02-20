﻿using DS4Windows.Client.Core.DependencyInjection;
using DS4Windows.Shared.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using JetBrains.Annotations;

namespace DS4Windows.Shared.Common
{
    [UsedImplicitly]
    public class CommonRegistrar : IServiceRegistrar
    {
        public void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IGlobalStateService, GlobalStateService>();
        }

        public void Initialize(IServiceProvider services)
        {
        }
    }
}
