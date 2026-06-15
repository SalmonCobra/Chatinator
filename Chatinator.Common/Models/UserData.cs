using System.Net.Sockets;

namespace Chatinator.Common.Models;

public class UserData
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Salt { get; set; }
    public string? Hash { get; set; }
    public required TcpClient Socket { get; set; }
}