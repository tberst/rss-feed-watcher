using EzTvWatcher.Code;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTvWatcher.Test
{
    public static class ConfigHelper
    {

        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static GoogleApiConfig GetGoogleApplicationConfiguration(string outputPath)
        {
            var configuration = new GoogleApiConfig();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("GoogleApiConfig")
                .Bind(configuration);

            return configuration;
        }
    }
}
