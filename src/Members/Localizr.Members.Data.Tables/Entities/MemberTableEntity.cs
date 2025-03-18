using Azure;
using Azure.Data.Tables;

namespace Localizr.Members.Data.Tables.Entities;

public class MemberTableEntity : ITableEntity
{
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
