using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Core.Abstractions.Services;
using Localizr.Core.Services;
using Localizr.Members.Abstractions.DataTransferObjects;
using Localizr.Members.Features.MemberGet;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Localizr.Members.Api.Endpoints;

public static class MemberEndpoints
{
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/members").WithTags("Members");

        group.MapGet("/", MemberGet)
            .WithName(nameof(MemberGet))
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", MemberCreate)
            .RequireAuthorization()
            .WithName(nameof(MemberCreate))
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<Results<Ok<LocalizrResponse<MemberDetailsResponse>>, ValidationProblem>> MemberGet(
        [FromServices] IQueryHandler<MemberGetQuery, LocalizrResponse<MemberDetailsResponse>> handler,
        [FromServices] IOidcTokensService oidcTokensService)
    {
        var query = new MemberGetQuery("123");
        var response = await handler.HandleAsync(query, CancellationToken.None);
        return TypedResults.Ok(response);
    }


    private static async Task<Results<Ok<LocalizrResponse<MemberDetailsResponse>>, ValidationProblem>> MemberCreate(
        [FromBody] MemberCreateCommand command, 
        [FromServices] ICommandHandler<MemberCreateCommand, LocalizrResponse<MemberDetailsResponse>> handler,
        [FromServices] IOidcTokensService oidcTokensService)
    {
        var userSubjectId = oidcTokensService.GetSubjectId();
        command = command with { SubjectId= userSubjectId };
        var response = await handler.HandleAsync(command, CancellationToken.None);
        return TypedResults.Ok(response);
    }
}