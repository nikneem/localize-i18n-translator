using Localizr.Core.Configuration;
using System.Text.Json;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Localizr.Core.ExtensionMethods;

public static class AppHostBuilderExtensions
{
    public static IHostApplicationBuilder AddLocalizrCoreServices(this IHostApplicationBuilder builder)
    {
        var azureServicesSection = builder.Configuration.GetSection(CosmosDbConfiguration.DefaultSectionName);
            builder.Services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
        builder.Services.AddOptions<CosmosDbConfiguration>().Bind(azureServicesSection).ValidateOnStart();

        var configuration = azureServicesSection.Get<CosmosDbConfiguration>();

        builder.AddAzureCosmosClient("projects",
            configureClientOptions: options =>
            {
                options.UseSystemTextJsonSerializerWithOptions = JsonSerializerOptions.Web;
            },
            configureSettings: settings => { settings.Credential = new DefaultAzureCredential(); });

        return builder;
    }

}