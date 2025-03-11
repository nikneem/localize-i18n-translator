using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Projects.Features.ProjectCreate;
using Localizr.Projects.Features.ProjectList;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Localizr.Projects.ExtensionMethods;

public static class AppHostBuilderExtensions
{
    public static IHostApplicationBuilder AddLocalizrProjects(this IHostApplicationBuilder builder)
    {
        // Adding queries
        builder.Services.AddScoped<IQueryHandler<ProjectListQuery, LocalizrListResponse<ProjectListItem>>, ProjectListHandler>();

        // Adding commands
        builder.Services.AddScoped<ICommandHandler<ProjectCreateCommand, LocalizrResponse<ProjectCreateDetailsResponse>>, ProjectCreateHandler>();
        return builder;
    }

}