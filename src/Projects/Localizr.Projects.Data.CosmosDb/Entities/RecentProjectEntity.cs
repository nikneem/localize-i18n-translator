using Localizr.Core.CosmosDb;

namespace Localizr.Projects.Data.CosmosDb.Entities;

public record RecentProjectEntity(Guid Id, string Name, DateTimeOffset LastOpenedOn) : ProjectEntityBase(Id, nameof(RecentProjectEntity));
