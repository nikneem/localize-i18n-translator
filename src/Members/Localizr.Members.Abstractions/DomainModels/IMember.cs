using HexMaster.DomainDrivenDesign.Abstractions;

namespace Localizr.Members.Abstractions.DomainModels;

public interface IMember : IDomainModel<Guid>
{
    string SubjectId { get; }
    string DisplayName { get; }
    string EmailAddress { get; }
    string? ProfilePicture { get; }
}