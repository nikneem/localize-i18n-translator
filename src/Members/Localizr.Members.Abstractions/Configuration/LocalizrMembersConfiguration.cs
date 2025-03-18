namespace Localizr.Members.Abstractions.Configuration;

public class LocalizrMembersConfiguration
{
    public const string DefaultSectionName = "LocalizrMembers";

    public string? StorageAccountName { get; set; }
    public string? MembersTableName { get; set; }
}