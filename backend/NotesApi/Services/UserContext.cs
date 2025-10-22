using Microsoft.AspNetCore.Http;

namespace NotesApi.Services;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is null) return null;
            if (context.Request.Headers.TryGetValue("X-User-Id", out var values))
            {
                return values.FirstOrDefault();
            }
            return null;
        }
    }
}


