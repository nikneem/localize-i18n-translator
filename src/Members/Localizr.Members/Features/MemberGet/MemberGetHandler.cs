using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.Mappings;
using Localizr.Members.Abstractions.Repositories;
using Localizr.Members.Features.MemberCreate;
using Microsoft.Extensions.Logging;

namespace Localizr.Members.Features.MemberGet;

public class MemberGetHandler(IMembersRepository membersRepository, ILogger<MemberCreateHandler> logger) : IQueryHandler<MemberGetQuery, LocalizrResponse<MemberDetailsResponse>>
{
    public async ValueTask<LocalizrResponse<MemberDetailsResponse>> HandleAsync(MemberGetQuery query, CancellationToken cancellationToken)
    {
        var member = await membersRepository.Get(query.SubjectId, cancellationToken);
        if (member == null)
        {
            return LocalizrResponse<MemberDetailsResponse>.Fail("Member could not be found");
        }
        var responseModel = member.ToDetailsResponse();
        return LocalizrResponse<MemberDetailsResponse>.Success(responseModel);
    }
}