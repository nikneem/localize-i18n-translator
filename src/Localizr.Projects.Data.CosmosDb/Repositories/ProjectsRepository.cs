using System.Net;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localizr.Core.Configuration;
using Localizr.Projects.Abstractions.DomainModels;
using Localizr.Projects.Abstractions.Repositories;
using Localizr.Projects.Data.CosmosDb.Entities;
using Localizr.Projects.Data.CosmosDb.Mappings;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace Localizr.Projects.Data.CosmosDb.Repositories;

public class ProjectsRepository(CosmosClient client, IOptions<CosmosDbConfiguration> cosmosConfiguration) : IProjectsRepository
{
    public async  Task<bool> Save(IProject project, CancellationToken cancellationToken)
    {
        if (project.TrackingState == TrackingState.New)
        {
            var entity = project.ToEntity();
            var container = GetContainer();
            var response = await container.CreateItemAsync(entity, cancellationToken: cancellationToken);
            return response.StatusCode == HttpStatusCode.Created;
        }

        return false;

    }
    public async Task<List<IProject>> List(string? name, CancellationToken cancellationToken)
    {
        var container = GetContainer();
        var query = container
            .GetItemLinqQueryable<ProjectEntity>()
            .Where(x => x.EntityType == nameof(ProjectEntity));
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }
        var iterator = query.ToFeedIterator();

        var list = new List<IProject>();
        while (iterator.HasMoreResults)
        {
            var batch = await iterator.ReadNextAsync(cancellationToken);
            list.AddRange(batch.ToDomainModels());
        }

        return list;
    }

    private Container GetContainer()
    {
        var configuration = cosmosConfiguration.Value;
        var database = client.GetDatabase(configuration.DatabaseName);
        return database.GetContainer(configuration.ProjectsContainer);
    }
}