using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;

namespace Localizr.Translations.DomainModels;

public class ChildTranslation : DomainModel<Guid>
{

    private readonly List<ChildTranslation> _childTranslations;
    private readonly List<TranslationValue> _translationValues;

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

    public ChildTranslation(Guid id,
        string key,
        string fullNodeKey,
        List<ChildTranslation> childTranslations,
        List<TranslationValue> values,
        bool isChecked,
        DateTimeOffset createdOn,
        DateTimeOffset? lastModifiedOn) : base(id)
    {
        _childTranslations = childTranslations;
        _translationValues = values;
        Key = key;
        FullNodeKey = fullNodeKey;
        IsChecked = isChecked;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    private ChildTranslation(string key) : base(Guid.NewGuid(), TrackingState.New)
    {
        Key = key;
        FullNodeKey = key;
        IsChecked = false;
        CreatedOn = DateTimeOffset.UtcNow;
        _childTranslations = [];
        _translationValues = [];
    }

    internal static ChildTranslation Create(string key)
    {
        return new ChildTranslation(key);
    }
}