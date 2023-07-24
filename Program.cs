using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Console.WriteLine("Created HostBuilder");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    Console.WriteLine("ConfigureLogging");
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();
                    if (!context.HostingEnvironment.IsDevelopment())
                    {
                        Console.WriteLine("Get AzureKeyVaultEndpoint");
                        var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];
                        Console.WriteLine("keyVaultEndpoint : " + keyVaultEndpoint);
                        if (!string.IsNullOrEmpty(keyVaultEndpoint))
                        {
                            var azureServiceTokenProvider = new AzureServiceTokenProvider();
                            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                            config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                        }
                        UpdateNLogConfig(config.Build(), context.HostingEnvironment);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Console.WriteLine("Startup");
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();

        private static void UpdateNLogConfig(IConfigurationRoot configuration, IHostEnvironment env)
        {
            Console.WriteLine("UpdateNLogConfig");
            // DEV & UAT
            //var storageConnectionString = configuration.GetSection("dev-uat-storage-saleonmob").Get<string>();
            // PROD
            var storageConnectionString = configuration.GetSection("prd-saleonmobile-storage").Get<string>();
            Console.WriteLine("storageConnectionString:" + storageConnectionString);
            GlobalDiagnosticsContext.Set("StorageConnectionString", storageConnectionString);
            var configFile = env.IsDevelopment() ? "nlog.{env.EnvironmentName}.config" : "nlog.config";
            Console.WriteLine("configFile:" + configFile);
            LogManager.Configuration = LogManager.LoadConfiguration(configFile).Configuration;
        }
    }
}
