using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Localizr.Members.Api.Endpoints;

public static class MemberEndpoints
{
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/members").WithTags("Members");

        group.MapPost("/", MemberCreate)
            .WithName(nameof(MemberCreate))
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<Results<Created, ValidationProblem>> MemberCreate(
        [FromBody] MemberCreateCommand command, 
        [FromServices] ICommandHandler<MemberCreateCommand, LocalizrResponse<MemberDetailsResponse>> handler)
    {
        var response = await handler.HandleAsync(command, CancellationToken.None);

        return TypedResults.Created($"/members/{response.Data.Id}");

        
    }
}