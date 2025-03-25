using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localize.Organizations.Abstractions.ValueObjects;

namespace Localize.Organizations.DomainModels;

public class Organization : DomainModel<Guid>
{

    private readonly List<OrganizationMember> _organizationMembers;
    private readonly List<OrganizationInvitation> _invitations;


    public string DisplayName { get; private set; }

    public IReadOnlyList<OrganizationMember> Members => _organizationMembers.AsReadOnly();
    public IReadOnlyList<OrganizationInvitation> Invitations => _invitations.AsReadOnly();


    public void AddMember(OrganizationMember value)
    {
        if (Members.Any(m => m.Id == value.Id))
        {
            // Todo: throw exception
        }

        _organizationMembers.Add(value);
        SetState(TrackingState.Modified);
    }
    public void RemoveMember(Guid id)
    {
        var value = _organizationMembers.FirstOrDefault(x => x.Id == id);
        if (value != null)
        {
            _organizationMembers.Remove(value);
        }
    }
    public void RemoveMember(OrganizationMember value)
    {
        if (_organizationMembers.All(m => m.Id == value.Id || m.Role.Id != OrganizationMemberRole.Admin.Id))
        {
            // todo: throw exception / This would be the last admin
        }
        _organizationMembers.Remove(value);
    }

    public void UpdateMember(OrganizationMember value)
    {
        if (value.Role != OrganizationMemberRole.Admin &&
            _organizationMembers.All(m => m.Id == value.Id || m.Role.Id != OrganizationMemberRole.Admin.Id))
        {
            // todo: role possibly changed, this would remove the last admin
        }
        var existing = _organizationMembers.FirstOrDefault(x => x.Id == value.Id);
        if (existing != null)
        {
            _organizationMembers.Remove(existing);
            _organizationMembers.Add(value);
            SetState(TrackingState.Modified);
        }

    }



    public Organization(Guid id) : base(id)
    {
    }

    public Organization(Guid id, TrackingState state) : base(id, state)
    {
    }
}