using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;

namespace Localizr.Translations.DomainModels;

public class ChildTranslation : DomainModel<Guid>
{

    private readonly List<ChildTranslation> _childTranslations;

    public string Key { get; private set; }
    public string FullNodeKey { get; private set; }
    public string? Value { get; private set; }
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

    public void SetValue(string? value)
    {
        if (!Equals(value, Value))
        {
            IsChecked = false;
            Value = value;
            SetModified();
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
            existing.SetValue(translation.Value);
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
        string? value,
        List<ChildTranslation> childTranslations,
        bool isChecked,
        DateTimeOffset createdOn,
        DateTimeOffset? lastModifiedOn) : base(id)
    {
        _childTranslations = childTranslations;
        Key = key;
        FullNodeKey = fullNodeKey;
        Value = value;
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
    }

    internal static ChildTranslation Create(string key)
    {
        return new ChildTranslation(key);
    }
}