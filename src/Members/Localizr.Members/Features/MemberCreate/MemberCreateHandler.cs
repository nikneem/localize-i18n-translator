using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.Mappings;
using Localizr.Members.Abstractions.Repositories;
using Localizr.Members.DomainModels;
using Microsoft.Extensions.Logging;

namespace Localizr.Members.Features.MemberCreate;

public class MemberCreateHandler(
    IMembersRepository membersRepository, 
    ILogger<MemberCreateHandler> logger) : ICommandHandler<MemberCreateCommand, LocalizrResponse<MemberDetailsResponse>>
{
    public async ValueTask<LocalizrResponse<MemberDetailsResponse>> HandleAsync(MemberCreateCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling incoming message for member registration with command {commandId}", command.CommandId);
        var domainModel = Member.FromCreateCommand(command);

        var saveResult = await membersRepository.Save(domainModel, cancellationToken);
        if (saveResult)
        {
            logger.LogInformation("Member {memberId} has been saved", domainModel.Id);
            return LocalizrResponse<MemberDetailsResponse>.Success(domainModel.ToDetailsResponse());
        }
        return LocalizrResponse<MemberDetailsResponse>.Fail("Failed to save the member");
    }
}