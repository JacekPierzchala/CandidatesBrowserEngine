using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
                {

                    var builtConfig = config.Build();
                    var keyVaultEndpoint = $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/";
                    if (!String.IsNullOrEmpty(keyVaultEndpoint) && !String.IsNullOrEmpty(builtConfig["KeyVaultName"]))
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                          new KeyVaultClient.AuthenticationCallback(
                              azureServiceTokenProvider.KeyVaultTokenCallback));

                         config.AddAzureKeyVault(new Uri("https://candidatesvault.vault.azure.net/"), new DefaultAzureCredential());
                    }

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
