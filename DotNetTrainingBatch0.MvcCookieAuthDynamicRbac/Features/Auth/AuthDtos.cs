namespace DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Features.Auth;

public class AuthRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class AuthResponse
{
    public string Role { get; set; } = "";
}
