using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localizr.Members.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.DomainModels;

namespace Localizr.Members.DomainModels;

public class Member : DomainModel<Guid>, IMember
{
    public string SubjectId { get; }
    public string DisplayName { get; private set; }
    public string EmailAddress { get; private set; }
    public bool EmailAddressVerified { get; private set; }
    public string? ProfilePicture { get; private set; }


    public Member(Guid id, string subjectId, string displayName, string emailAddress, bool emailAddressVerified, string? profilePicture) : base(id)
    {
        SubjectId = subjectId;
        DisplayName = displayName;
        EmailAddress = emailAddress;
        ProfilePicture = profilePicture;
    }

    private Member(string subjectId, string displayName, string emailAddress, bool emailAddressVerified, string? profilePicture) : base(Guid.NewGuid(), TrackingState.New)
    {
        SubjectId = subjectId;
        DisplayName = displayName;
        EmailAddress = emailAddress;
        ProfilePicture = profilePicture;
    }

    internal static IMember FromCreateCommand(MemberCreateCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.SubjectId))
        {
            throw new Exception("Subject ID cannot be null while creating new member");
        }

        return new Member(command.SubjectId, command.DisplayName, command.EmailAddress, command.EmailAddressVerified??false,  command.ProfilePicture);
    }
}