using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localize.Organizations.Abstractions.ValueObjects;

namespace Localize.Organizations.DomainModels;

public class OrganizationMember: DomainModel<Guid>
{

    public string DisplayName { get; }
    public string EmailAddress { get; }
    public OrganizationMemberRole Role { get; private set; }
    public string? ProfilePicture { get; }




    public OrganizationMember(Guid id, 
        string displayName,
        string emailAddress,
        OrganizationMemberRole role,
        string profilePicture) : base(id)
    {
        DisplayName = displayName;
        EmailAddress = emailAddress;
        Role = role;
        ProfilePicture = profilePicture;
    }

    private OrganizationMember(string displayName, OrganizationMemberRole role) : base(Guid.NewGuid(), TrackingState.New)
    {
        DisplayName = displayName;
        Role = role;
    }
}