using System.Security.Claims;

namespace NotesApi.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    public string GetUsername()
    {
        var usernameClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        return usernameClaim?.Value ?? throw new UnauthorizedAccessException("Username not found in token");
    }
}