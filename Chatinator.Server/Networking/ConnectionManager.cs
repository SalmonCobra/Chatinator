using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Chatinator.Common.Models;
using Chatinator.Common.Networking;

namespace Chatinator.Server.Networking;

// a class for handling client connections
public class ConnectionManager
{
    public static async Task ClientHandler(UserData client)
    {
        var cert = new X509Certificate2("/home/benjamin/RiderProjects/Chatinator/Chatinator.Server/server.pfx", "password123");
        var sslStream = new SslStream(client.Socket.GetStream());
        await sslStream.AuthenticateAsServerAsync(cert);
        var reader = new StreamReader(sslStream);
        var writer = new StreamWriter(sslStream);
        Console.WriteLine($"Client {client.Id} connected");
        //UserStore.Users.Add(client);


        PacketService.SendSimplePacket("Welcome to Chatinator!", writer);
        _ = PacketService.PacketReader(reader); // Start reading packets from client in the background until connection dropped
    }
}