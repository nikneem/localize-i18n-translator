using Localizr.Core.CosmosDb;

namespace Localizr.Translations.Data.Cosmos.Entities;

public record TranslationRootEntity(
    Guid Id, 
    string LanguageId, 
    bool IsBaseTranslations, 
    Guid ProjectId, 
    string EntityType) : ProjectEntityBase(ProjectId, EntityType)
{
    
}