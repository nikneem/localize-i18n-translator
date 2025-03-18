using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var storageAccount = builder.AddAzureStorage("storage");
var membersTable = storageAccount.AddTables("members");

var cosmos = builder.AddAzureCosmosDB("cosmos")
    .WithHttpEndpoint(51234, 1234, "explorer-port")
    .WithExternalHttpEndpoints();

if (builder.Environment.IsDevelopment())
{
#pragma warning disable ASPIRECOSMOSDB001
    cosmos.RunAsPreviewEmulator(options =>
    {
        options.WithLifetime(ContainerLifetime.Persistent);
    });
#pragma warning restore ASPIRECOSMOSDB001

    storageAccount.RunAsEmulator(opts =>
    {
        opts.WithLifetime(ContainerLifetime.Persistent);
    });

}

var database = cosmos.AddCosmosDatabase("localizr");
var container = database.AddContainer("projects", "/projectId");

var membersApi = builder.AddProject<Projects.Localizr_Members_Api>("localizr-members-api")
    .WaitFor(membersTable)
    .WithReference(membersTable)
    .WithEnvironment("LocalizrMembers:StorageAccountName", storageAccount.Resource.Name)
    .WithEnvironment("LocalizrMembers:MembersTableName", "members");


var projectsApi = builder.AddProject<Projects.Localizr_Projects_Api>("localizr-projects-api")
    .WaitFor(cosmos)
    .WithReference(container)
    .WithEnvironment("CosmosDb:DatabaseName", "localizr")
    .WithEnvironment("CosmosDb:ProjectsContainer", "projects");

var translationsApi = builder.AddProject<Projects.Localizr_Translations_Api>("localizr-translations-api")
    .WaitFor(cosmos)
    .WithReference(container)
    .WithEnvironment("CosmosDb:DatabaseName", "localizr")
    .WithEnvironment("CosmosDb:ProjectsContainer", "projects");

var organizationsApi = builder.AddProject<Projects.Localizr_Organizations_Api>("localizr-organizations-api")
    .WaitFor(cosmos)
    .WithReference(container)
    .WithEnvironment("CosmosDb:DatabaseName", "localizr")
    .WithEnvironment("CosmosDb:ProjectsContainer", "projects");


var proxyApi = builder.AddProject<Projects.Localizr_Proxy_Api>("localizr-proxy-api")
    .WaitFor(projectsApi)
    .WaitFor(translationsApi)
    .WaitFor(organizationsApi)
    .WaitFor(membersApi)
    .WithReference(projectsApi)
    .WithReference(translationsApi)
    .WithReference(organizationsApi)
    .WithReference(membersApi);

builder.Build().Run();
