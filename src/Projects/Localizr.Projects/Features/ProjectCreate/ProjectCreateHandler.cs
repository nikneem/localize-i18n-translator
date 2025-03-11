using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Projects.Abstractions.Repositories;
using Localizr.Projects.DomainModels;
using Microsoft.Extensions.Logging;

namespace Localizr.Projects.Features.ProjectCreate;

public class ProjectCreateHandler(
    IProjectsRepository projectsRepository,
    ILogger<ProjectCreateHandler> logger
    ) : ICommandHandler<ProjectCreateCommand, LocalizrResponse<ProjectCreateDetailsResponse>>
{
    public async ValueTask<LocalizrResponse<ProjectCreateDetailsResponse>> HandleAsync(ProjectCreateCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling project create command for project {projectName}", command.Name);
        var project = Project.FromCommand(command);

        if (await projectsRepository.Save(project, cancellationToken))
        {
            var responseModel = new ProjectCreateDetailsResponse(
                project.Id,
                project.Name,
                project.Description,
                project.DefaultLanguage,
                project.SupportedLanguages.ToList(),
                project.CreatedOn
            );
            return LocalizrResponse<ProjectCreateDetailsResponse>.Success(responseModel);
        }

        return LocalizrResponse<ProjectCreateDetailsResponse>.Fail("Failed to save project");
    }
}