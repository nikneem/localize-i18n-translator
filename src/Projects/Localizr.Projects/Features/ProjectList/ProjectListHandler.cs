using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Projects.Abstractions.DomainModels;
using Localizr.Projects.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace Localizr.Projects.Features.ProjectList;

public class ProjectListHandler (IProjectsRepository projectsRepository, ILogger<ProjectListHandler> logger)  : IQueryHandler<ProjectListQuery, LocalizrListResponse<ProjectListItem>>
{
    public async ValueTask<LocalizrListResponse<ProjectListItem>> HandleAsync(ProjectListQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling project list query");
        
        var domainModels = await projectsRepository.List(query.Name, cancellationToken);
        var responseItems = domainModels.Select(dm => new ProjectListItem(dm.Id, dm.Name, dm.DefaultLanguage, dm.CreatedOn)).ToList();
        return LocalizrListResponse<ProjectListItem>.Success(responseItems);


    }
}