using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Projects.Features.ProjectCreate;
using Localizr.Projects.Features.ProjectList;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Localizr.Projects.Api.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/projects").WithTags("Projects");

        group.MapGet("/", ProjectList)
            .WithName(nameof(ProjectList))
            .Produces(StatusCodes.Status200OK);

        group.MapPost("/", ProjectCreate)
            .WithName(nameof(ProjectCreate))
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> ProjectList(
        [AsParameters] ProjectListQuery query,
        [FromServices] IQueryHandler<ProjectListQuery, LocalizrListResponse<ProjectListItem>> handler)
    {
        var response = await handler.HandleAsync(query, CancellationToken.None);
        return Results.Ok(response);
    }


    private static async Task<Results<Created, ValidationProblem>> ProjectCreate(
        [FromBody] ProjectCreateCommand command, 
        [FromServices] ICommandHandler<ProjectCreateCommand, LocalizrResponse<ProjectCreateDetailsResponse>> handler)
    {
        var response = await handler.HandleAsync(command, CancellationToken.None);

        return TypedResults.Created($"/projects/{response.Data.Id}");

        
    }
}