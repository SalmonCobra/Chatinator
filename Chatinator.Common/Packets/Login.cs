namespace Chatinator.Common.Packets;

public class LoginPacket
{
    public string Type { get; } = "Login";
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool NewUser { get; set; }
}

public class LoginResponse
{
    public string Type { get; } = "LoginResponse";
    public bool Success { get; set; }
    public string? FailureReason { get; set; }
}