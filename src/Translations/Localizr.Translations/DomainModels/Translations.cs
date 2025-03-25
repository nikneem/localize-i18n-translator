using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;

namespace Localizr.Translations.DomainModels;

public class Translations : DomainModel<Guid>
{

    private readonly List<ChildTranslation> _childTranslations;
    private readonly List<TranslationValue> _translationValues;

    public Guid ProjectId { get; }
    public bool IsDefault { get; }
    public string Key { get; private set; }
    public string FullNodeKey { get; private set; }
    public IReadOnlyList<TranslationValue> Values => _translationValues.AsReadOnly();
    public IReadOnlyList<ChildTranslation> Children => _childTranslations.AsReadOnly();
    public bool IsChecked { get; private set; }
    public DateTimeOffset CreatedOn { get; }
    public DateTimeOffset? LastModifiedOn { get; private set; }

    public void SetKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception("Failed");
        }

        if (!Equals(Key, value))
        {
            Key = value;
            FullNodeKey = value;
            SetModified();
        }
    }

    public void SetValue(string language, string? value)
    {
        var translation = _translationValues.FirstOrDefault(tv => tv.LanguageId == language);
        if (translation != null)
        {
            translation.SetValue(value);
        }
        else
        {
            translation = new TranslationValue(language, value);
            _translationValues.Add(translation);
        }
    }

    public void Checked()
    {
        if (!IsChecked)
        {
            IsChecked = true;
            SetModified();
        }
    }

    public ChildTranslation AddChild(string key)
    {
        var child = ChildTranslation.Create(key);
        _childTranslations.Add(child);
        SetModified();
        return child;
    }

    public void UpdateChild(ChildTranslation translation)
    {
        var existing = _childTranslations.FirstOrDefault(ct => ct.Id == translation.Id);
        if (existing != null)
        {
            existing.SetKey(translation.Key);
            SetModified();
        }
    }

    public void RemoveChild(Guid childId)
    {
        var existing = _childTranslations.FirstOrDefault(ct => ct.Id == childId);
        if (existing != null)
        {
            _childTranslations.Remove(existing);
            SetModified();
        }
    }

    private void SetModified()
    {
        SetState(TrackingState.Modified);
        LastModifiedOn = DateTimeOffset.UtcNow;
    }

    public Translations(Guid id,
        Guid projectId,
        bool isDefault,
        string key, 
        string fullNodeKey,
        List<TranslationValue> values,
        List<ChildTranslation> childTranslations,
        bool isChecked, 
        DateTimeOffset createdOn, 
        DateTimeOffset? lastModifiedOn) : base(id)
    {
        _childTranslations = childTranslations;
        _translationValues = values;
        ProjectId = projectId;
        Key = key;
        FullNodeKey = fullNodeKey;
        IsChecked = isChecked;
        IsDefault = isDefault;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    private Translations(Guid projectId, bool isDefault, string key) : base(Guid.NewGuid(), TrackingState.New)
    {
        ProjectId = projectId;
        IsDefault = isDefault;
        Key = key;
        FullNodeKey = key;
        IsChecked = false;
        CreatedOn = DateTimeOffset.UtcNow;
        _childTranslations = [];
        _translationValues = [];
    }

    internal static Translations Create(Guid projectId, string languageId, bool isDefault, string key)
    {
        return new Translations(projectId,  isDefault, key);
    }


}