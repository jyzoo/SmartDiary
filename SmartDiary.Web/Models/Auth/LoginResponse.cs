namespace SmartDiary.Web.Models.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
}