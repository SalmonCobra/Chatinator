using Chatinator.Client.Networking;
using Chatinator.Common.Networking;

namespace Chatinator.Client;

internal class Program
{
    public static readonly string hostname = "127.0.0.1";
    public static NetworkClient TcpClient = new();

    public static async Task Main(string[] args)
    {
        //68.189.40.187
        await TcpClient.Connect(hostname, 38120);
        //Console.WriteLine(Authentication.LoginAuthenticator.LoginCli(TcpClient.));
        PacketService.SendSimplePacket("yo", NetworkClient.Writer);
        Thread.Sleep(Timeout.Infinite);
    }
}