using Azure.Identity;
using Localizr.Members.Abstractions.Configuration;
using Localizr.Members.Abstractions.Repositories;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Localizr.Members.Data.Tables.ExtensionMethods;

public static  class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder WithLocalizrMembersTableStorage(this IHostApplicationBuilder builder)
    {
        var identity = new DefaultAzureCredential();
        var configSection = builder.Configuration.GetRequiredSection(LocalizrMembersConfiguration.DefaultSectionName);

        var configurationValue = configSection.Get<LocalizrMembersConfiguration>();
        if (configurationValue == null)
        {
            throw new Exception($"Reviews service misconfiguration! Missing configuration section '{LocalizrMembersConfiguration.DefaultSectionName}'.");
        }
        builder.Services.AddAzureClients(azure =>
        {
            azure.UseCredential(identity);
            builder.AddAzureTableClient("members");
            //azure.AddTableServiceClient(new Uri($"https://{configurationValue.StorageAccountName}.table.core.windows.net"));
        });

        builder.Services.AddScoped<IMembersRepository, MembersRepository>();
        builder.Services.AddOptions<LocalizrMembersConfiguration>().Bind(configSection).ValidateOnStart();
        //builder.Services.AddScoped<IDirtyReviewsRepository, DirtyReviewsRepository>();
        //builder.Services.AddScoped<ICleanReviewsRepository, CleanReviewsRepository>();

        return builder;
    }
}