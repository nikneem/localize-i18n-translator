using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Projects.Abstractions.Repositories;
using Localizr.Projects.Features.ProjectList;
using Microsoft.Extensions.Logging;

namespace Localizr.Projects.Features.ProjectGet;

public class ProjectGetHandler(IProjectsRepository projectsRepository, ILogger<ProjectListHandler> logger) : IQueryHandler<ProjectGetQuery, LocalizrResponse<ProjectDetailsResponse>>
{
    public async ValueTask<LocalizrResponse<ProjectDetailsResponse>> HandleAsync(ProjectGetQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting single project details with project ID {projectId} from the projects repository", query.ProjectId);
        var result = await projectsRepository.Get(query.ProjectId, cancellationToken);
        var response = new ProjectDetailsResponse(
            result.Id, 
            result.Name, 
            result.DefaultLanguage, 
            result.SupportedLanguages.ToList(),
            result.CreatedOn, 
            result.LastModifiedOn);

        return LocalizrResponse<ProjectDetailsResponse>.Success(response);
    }
}