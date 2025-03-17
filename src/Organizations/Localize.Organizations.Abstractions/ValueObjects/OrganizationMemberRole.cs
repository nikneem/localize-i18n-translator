namespace Localize.Organizations.Abstractions.ValueObjects;

public sealed class OrganizationMemberRole(string id, string name)
{
    public static OrganizationMemberRole Admin = new(nameof(Admin).ToLowerInvariant(), nameof(Admin));
    public static OrganizationMemberRole Auditor = new(nameof(Auditor).ToLowerInvariant(), nameof(Auditor));
    public static OrganizationMemberRole Translator = new(nameof(Translator).ToLowerInvariant(), nameof(Translator));

    public static List<OrganizationMemberRole> AllRoles =
    [
        Admin,
        Auditor,
        Translator
    ];

    public static OrganizationMemberRole Find(string id)
    {
        return AllRoles.First(x => x.Id == id);
    }

    public string Id { get; } = id;
    public string Name { get; } = name;

}
