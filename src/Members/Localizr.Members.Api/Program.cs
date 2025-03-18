using Localizr.Members.Api.Endpoints;
using Localizr.Members.Data.Tables.ExtensionMethods;
using Localizr.Members.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddLocalizrMembers()
    .WithLocalizrMembersTableStorage();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


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


app.Run();
