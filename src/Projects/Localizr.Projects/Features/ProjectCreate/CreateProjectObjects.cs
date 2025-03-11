using Localizr.Core.Cqrs;

namespace Localizr.Projects.Features.ProjectCreate;

public record ProjectCreateCommand(
    string Name, 
    string? Description,
    string DefaultLanguage,
    List<string> SupportedLanguages) : CommandBase;

public record ProjectCreateDetailsResponse(
    Guid Id,
    string Name,
    string? Description,
    string DefaultLanguage,
    List<string> SupportedLanguages,
    DateTimeOffset CreatedOn);