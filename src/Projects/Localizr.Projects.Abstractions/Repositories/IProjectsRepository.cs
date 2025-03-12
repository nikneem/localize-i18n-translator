using Localizr.Projects.Abstractions.DomainModels;

namespace Localizr.Projects.Abstractions.Repositories;

public interface IProjectsRepository
{
    Task<bool> Save(IProject project, CancellationToken cancellationToken);

    Task<List<IProject>> List(string? name, CancellationToken cancellationToken);
    Task<IProject> Get(Guid projectId, CancellationToken cancellationToken);
}