using System.Text.Json;
using Azure.Identity;
using Localizr.Projects.Abstractions.Repositories;
using Localizr.Projects.Data.CosmosDb.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Localizr.Projects.Data.CosmosDb.ExtensionMethods;

public static class AppHostBuilderExtensions
{
    public static IHostApplicationBuilder WithCosmosProjectsRepository(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
        return builder;
    }

}