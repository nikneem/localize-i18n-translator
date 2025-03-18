using Localizr.Core.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Localizr.Core.Services;

public class OidcTokensService(IHttpContextAccessor httpContextAccessor) : IOidcTokensService
{

    public string GetSubjectId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity is { IsAuthenticated: true })
        {
            var subject = user.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"));
            if (subject != null)
            {
                return subject.Value;
            }
        }

        throw new Exception("Failed to get subject id");
    }
}