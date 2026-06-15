using System.Net;
using System.Net.Sockets;
using Chatinator.Common.Models;

namespace Chatinator.Server.Networking;

public class ChatinatorServer
{
    public TcpListener TcpServer = new(IPAddress.Any, 38120);

    public async Task RunServer()
    {
        TcpServer.Start();
        Console.WriteLine("TcpServer started on port 38120");


        while (true)
        {
            var client = await TcpServer.AcceptTcpClientAsync();
            _ = ConnectionManager.ClientHandler(new UserData { Socket = client });
        }
    }
}