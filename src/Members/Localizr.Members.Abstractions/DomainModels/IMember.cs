using HexMaster.DomainDrivenDesign.Abstractions;

namespace Localizr.Members.Abstractions.DomainModels;

public interface IMember : IDomainModel<Guid>
{
    string SubjectId { get; }
    string DisplayName { get; }
    string EmailAddress { get; }
    bool EmailAddressVerified { get; }
    string? ProfilePicture { get; }
}