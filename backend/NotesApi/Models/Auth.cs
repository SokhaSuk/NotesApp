namespace NotesApi.Models;

public class AuthResponse
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = true;
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
