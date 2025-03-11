using HexMaster.DomainDrivenDesign.Abstractions;

namespace Localizr.Projects.Abstractions.DomainModels;

public interface IProject : IDomainModel<Guid>
{
    string Name { get; }
    string? Description { get; }
    string DefaultLanguage { get; }
    IReadOnlyList<string> SupportedLanguages { get; }
    DateTimeOffset CreatedOn { get; }
    DateTimeOffset? LastModifiedOn { get; }
    void SetName(string value);
    void SetDescription(string? value);
    void SetDefaultLanguage(string value);
    void AddSupportedLanguage(string value);
    void RemoveSupportedLanguage(string value);
}