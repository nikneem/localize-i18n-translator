using Azure;
using Azure.Data.Tables;
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
                return new Member(Guid.NewGuid(), subjectId, "Henk", "henk@email.com", null);
            }
        }
        catch (RequestFailedException ex)
        {
            // The resource could not be found (e.g. member does not exist)
            logger.LogWarning(ex, "Tried to resolve member using subject ID, but the member could not be found");
        }
        return null;
    }

    public Task<bool> Save(IMember member, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}