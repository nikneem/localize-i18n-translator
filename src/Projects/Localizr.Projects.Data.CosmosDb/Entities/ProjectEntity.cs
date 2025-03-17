using Localizr.Core.CosmosDb;

namespace Localizr.Projects.Data.CosmosDb.Entities;

public record ProjectEntity(
    Guid Id,
    Guid ProjectId,
    string Name, 
    string? Description, 
    string DefaultLanguage, 
    List<string> SupportedLanguages, 
    DateTimeOffset CreatedOn, 
    DateTimeOffset? LastModifiedOn) : ProjectEntityBase(ProjectId, nameof(ProjectEntity));