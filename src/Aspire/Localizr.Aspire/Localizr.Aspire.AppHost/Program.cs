using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


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

}

var database = cosmos.AddCosmosDatabase("localizr");
var container = database.AddContainer("projects", "/projectId");

builder.AddProject<Projects.Localizr_Projects_Api>("localizr-projects-api")
    .WaitFor(cosmos)
    .WithReference(container)
    .WithEnvironment("CosmosDb:DatabaseName", "localizr")
    .WithEnvironment("CosmosDb:ProjectsContainer", "projects");


builder.Build().Run();
