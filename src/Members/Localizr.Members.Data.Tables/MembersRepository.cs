using Azure;
using Azure.Data.Tables;
using HexMaster.DomainDrivenDesign.ChangeTracking;
using Localizr.Members.Abstractions.Configuration;
using Localizr.Members.Abstractions.DomainModels;
using Localizr.Members.Abstractions.Repositories;
using Localizr.Members.Data.Tables.Entities;
using Localizr.Members.DomainModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Localizr.Members.Data.Tables;

public class MembersRepository(
    TableServiceClient client,
    ILogger<MembersRepository> logger,
    IOptions<LocalizrMembersConfiguration> config) : IMembersRepository
{

    private const string PartitionKey = "members";

    public async Task<IMember?> Get(string subjectId, CancellationToken cancellationToken)
    {
        var tableClient = client.GetTableClient(config.Value.MembersTableName);
        try
        {
            await tableClient.CreateIfNotExistsAsync(cancellationToken);
            var tableQueryResult =
                await tableClient.GetEntityAsync<MemberTableEntity>(PartitionKey, subjectId,
                    cancellationToken: cancellationToken);
            if (!tableQueryResult.GetRawResponse().IsError)
            {

                var entity = tableQueryResult.Value;
                return new Member(Guid.NewGuid(), subjectId, entity.DisplayName, entity.EmailAddress, entity.EmailAddressVerified, entity.ProfilePicture);
            }
        }
        catch (RequestFailedException ex)
        {
            // The resource could not be found (e.g. member does not exist)
            logger.LogWarning(ex, "Tried to resolve member using subject ID, but the member could not be found");
        }
        return null;
    }

    public async Task<bool> Save(IMember member, CancellationToken cancellationToken)
    {
        if (member.TrackingState == TrackingState.New ||
            member.TrackingState == TrackingState.Modified)
        {
            var membersEntity = new MemberTableEntity
            {
                PartitionKey = PartitionKey,
                RowKey = member.SubjectId,
                Id = member.Id,
                DisplayName = member.DisplayName,
                EmailAddress = member.EmailAddress,
                EmailAddressVerified = member.EmailAddressVerified,
                ProfilePicture = member.ProfilePicture,
                CreatedOn = DateTimeOffset.UtcNow,
            };

            var tableClient = client.GetTableClient(config.Value.MembersTableName);
            await tableClient.CreateIfNotExistsAsync(cancellationToken);
            var result = await tableClient.UpsertEntityAsync(membersEntity, TableUpdateMode.Replace, cancellationToken: cancellationToken);
            return !result.IsError;
        }
        // Nothing to be done
        return true;
    }
}