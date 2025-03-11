namespace Localizr.Core.Configuration;

public class CosmosDbConfiguration
{
    public const string DefaultSectionName = "CosmosDb";

    public string? Endpoint { get; set; }
    public string? DatabaseName { get; set; }
    public string? ProjectsContainer { get; set; }
}