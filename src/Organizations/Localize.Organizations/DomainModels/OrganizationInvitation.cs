using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localize.Organizations.Abstractions;
using Localize.Organizations.Abstractions.ValueObjects;
using Localizr.Core;

namespace Localize.Organizations.DomainModels;

public class OrganizationInvitation : DomainModel<Guid>
{

    public string EmailAddress { get; }
    public string InvitationCode { get; }
    public OrganizationMemberRole Role { get; private set; }
    public DateTimeOffset InvitedOn { get; }
    public DateTimeOffset? AcceptedOn { get; private set; }
    public DateTimeOffset? DeclinedOn { get; private set; }
    public DateTimeOffset ExpiresOn { get; }
    public bool IsExpired => ExpiresOn < DateTimeOffset.UtcNow;
    public bool IsAccepted => AcceptedOn.HasValue;
    public bool IsDeclined => DeclinedOn.HasValue;

    public OrganizationInvitation(
        Guid id, 
        string emailAddress, 
        string invitationCode,
        OrganizationMemberRole role, 
        DateTimeOffset invitedOn, 
        DateTimeOffset? acceptedOn, 
        DateTimeOffset? declinedOn,  
        DateTimeOffset expiresOn) : base(id)
    {
        EmailAddress = emailAddress;
        InvitationCode = invitationCode;
        Role = role;
        InvitedOn = invitedOn;
        AcceptedOn = acceptedOn;
        DeclinedOn = declinedOn;
        ExpiresOn = expiresOn;
    }

    public OrganizationInvitation(string emailAddress, OrganizationMemberRole role) : base(Guid.NewGuid(), TrackingState.New)
    {
        EmailAddress = emailAddress;
        Role = role;
        InvitationCode = Randomizer.GetRandomInvitationCode();
        InvitedOn = DateTimeOffset.UtcNow;
        ExpiresOn = DateTimeOffset.UtcNow.AddDays(OrganizationConstants.OrganizationInvitationValidityDays);
    }
}