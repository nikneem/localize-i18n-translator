using Azure;
using Azure.Data.Tables;

namespace Localizr.Members.Data.Tables.Entities;

public class MemberTableEntity : ITableEntity
{
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public Guid Id { get; set; }
    public required string DisplayName { get; set; }
    public required string EmailAddress { get; set; }
    public required bool EmailAddressVerified { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

}
