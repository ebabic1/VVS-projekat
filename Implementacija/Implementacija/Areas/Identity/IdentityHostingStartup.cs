﻿using System;
using Implementacija.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Implementacija.Areas.Identity.IdentityHostingStartup))]
namespace Implementacija.Areas.Identity
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}