using Localizr.Projects.Abstractions.DomainModels;
using Localizr.Projects.Data.CosmosDb.Entities;
using Localizr.Projects.DomainModels;

namespace Localizr.Projects.Data.CosmosDb.Mappings;

public static class ProjectMappings
{

    public static ProjectEntity ToEntity(this IProject project)
    {
        return new ProjectEntity(
            project.Id,
            project.Id,
            project.Name,
            project.Description,
            project.DefaultLanguage,
            project.SupportedLanguages.ToList(),
            project.CreatedOn,
            project.LastModifiedOn);
    }
    public static IProject ToDomainModel(this ProjectEntity entity)
    {
        return new Project(
            entity.ProjectId,
            entity.Name,
            entity.Description,
            entity.DefaultLanguage,
            entity.SupportedLanguages,
            entity.CreatedOn,
            entity.LastModifiedOn);
    }
    public static IEnumerable< IProject> ToDomainModels(this IEnumerable< ProjectEntity> entities)
    {
        return entities.Select(ToDomainModel);
    }

}