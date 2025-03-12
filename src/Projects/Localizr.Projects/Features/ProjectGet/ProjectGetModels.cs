using Localizr.Core.Abstractions.Cqrs;

namespace Localizr.Projects.Features.ProjectGet;

public record ProjectGetQuery(Guid ProjectId) : IQuery;
public record ProjectDetailsResponse(
    Guid Id,
    string Name,
    string DefaultLanguage,
    List<string> SupportedLanguages,
    DateTimeOffset CreatedOn,
    DateTimeOffset? LastModifiedOn
);