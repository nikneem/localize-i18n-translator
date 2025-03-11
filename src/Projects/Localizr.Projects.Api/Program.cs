using Localizr.Projects.Api.Endpoints;
using Localizr.Projects.Data.CosmosDb.ExtensionMethods;
using Localizr.Projects.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddLocalizrProjects()
    .WithCosmosProjectsRepository();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapProjectsEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

