using Localizr.Members.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.DomainModels;

namespace Localizr.Members.Abstractions.Mappings;

public static class MemberMappings
{


    public static MemberDetailsResponse ToDetailsResponse(this IMember member)
    {

        return new MemberDetailsResponse(member.Id, member.DisplayName, member.EmailAddress, member.ProfilePicture);

    }

}