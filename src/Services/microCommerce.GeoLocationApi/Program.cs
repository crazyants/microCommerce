﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace microCommerce.GeoLocationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddCommandLine(args)
            .AddEnvironmentVariables(prefix: "ASPNETCORE_")
            .Build();

            new WebHostBuilder()
            .UseConfiguration(config)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build()
            .Run();
        }
    }
}
