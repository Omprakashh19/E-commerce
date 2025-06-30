public class LoginResult
{
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsSuccess => !string.IsNullOrEmpty(Token);
}
