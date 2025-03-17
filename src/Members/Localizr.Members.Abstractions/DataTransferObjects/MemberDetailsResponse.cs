namespace Localizr.Members.Abstractions.DataTransferObjects;

public record MemberDetailsResponse(Guid Id, string DisplayName, string EmailAddress, string? ProfilePicture);