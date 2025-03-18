
using Localizr.Core.Abstractions.Cqrs;
using Localizr.Core.Abstractions.DataTransferObjects;
using Localizr.Members.Abstractions.DataTransferObjects;
using Localizr.Members.Features.MemberCreate;
using Localizr.Members.Features.MemberGet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Localizr.Members.ExtensionMethods;

public static  class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddLocalizrMembers(this IHostApplicationBuilder builder)
    {

        builder.Services.AddScoped<IQueryHandler<MemberGetQuery, LocalizrResponse<MemberDetailsResponse>>, MemberGetHandler>();

        builder.Services.AddScoped<ICommandHandler<MemberCreateCommand, LocalizrResponse<MemberDetailsResponse>>, MemberCreateHandler>();

        return builder;
    }
}