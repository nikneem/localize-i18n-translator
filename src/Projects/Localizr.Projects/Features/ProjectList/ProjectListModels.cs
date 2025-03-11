using Localizr.Core.Abstractions.Cqrs;

namespace Localizr.Projects.Features.ProjectList;

public record ProjectListItem(
    Guid Id,
    string Name,
    string DefaultLanguage,
    DateTimeOffset CreatedOn
);

public record ProjectListQuery( string? Name) : IQuery;