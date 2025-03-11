using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localizr.Projects.Abstractions.DomainModels;
using Localizr.Projects.Features.ProjectCreate;

namespace Localizr.Projects.DomainModels;

public class Project : DomainModel<Guid>, IProject
{
    private readonly List<string> _supportedLanguages;
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string DefaultLanguage { get; private set; }
    public IReadOnlyList<string> SupportedLanguages => _supportedLanguages.AsReadOnly();
    public DateTimeOffset CreatedOn { get; }
    public DateTimeOffset? LastModifiedOn { get; private set; }

    public void SetName(string value)
    {

        if (ValidateName(value))
        {
            Name = value;
            SetModified();
        }
    }
    private bool ValidateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AddValidationError("The name of the project cannot be null or empty");
            return false;
        }

        return !Equals(Name, value);
    }

    public void SetDescription(string? value)
    {
        if (ValidateDescription( value))
        {
            Description = value;
            SetModified();
        }
    }
    private bool ValidateDescription(string? value)
    {
        if (value is { Length: > 500 })
        {
            AddValidationError("The max length for a project description is 500 characters");
            return false;
        }
        return !Equals(Description, value);
    }

    public void SetDefaultLanguage(string value)
    {
        if (ValidateDefaultLanguage( value))
        {
            DefaultLanguage = value;
            SetModified();
        }
    }
    private bool ValidateDefaultLanguage(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AddValidationError("The default language of the project cannot be null or empty");
            return false;
        }
        return !Equals(DefaultLanguage, value);
    }

    public void AddSupportedLanguage(string value)
    {
        if (ValidateSupportedLanguage(value))
        {
            _supportedLanguages.Add(value);
            SetModified();
        }
    }
    private bool ValidateSupportedLanguage(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AddValidationError("Supported language cannot be null or empty.");
            return false;
        }
        return !_supportedLanguages.Contains(value);
    }

    public void RemoveSupportedLanguage(string value)
    {
        if (_supportedLanguages.Contains(value))
        {
            _supportedLanguages.Remove(value);
            SetModified();
        }
    }

    private void SetModified()
    {
        SetState(TrackingState.Modified);
        LastModifiedOn = DateTimeOffset.UtcNow;
    }

    public Project(
        Guid id,
        string name,
        string? description,
        string defaultLanguage,
        List<string> supportedLanguages,
        DateTimeOffset createdOn,
        DateTimeOffset? lastModifiedOn) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        DefaultLanguage = defaultLanguage ?? throw new ArgumentNullException(nameof(defaultLanguage));
        _supportedLanguages = supportedLanguages ?? throw new ArgumentNullException(nameof(supportedLanguages));
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    private Project(
        string name,
        string? description,
        string defaultLanguage,
        List<string> supportedLanguages) : base(Guid.NewGuid(), TrackingState.New)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        DefaultLanguage = defaultLanguage ?? throw new ArgumentNullException(nameof(defaultLanguage));
        _supportedLanguages = supportedLanguages ?? throw new ArgumentNullException(nameof(supportedLanguages));
        CreatedOn = DateTimeOffset.UtcNow;
    }

    internal static IProject FromCommand(ProjectCreateCommand command)
    {
        return new Project(command.Name,
            command.Description,
            command.DefaultLanguage,
            command.SupportedLanguages);
    }
}
