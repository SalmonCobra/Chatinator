using System.Net.Security;
using System.Net.Sockets;
using Chatinator.Common.Networking;

namespace Chatinator.Client.Networking;

public class NetworkClient
{
    public static TcpClient Socket;

    //public static NetworkStream Stream;
    public static SslStream Stream;
    public static StreamReader Reader;
    public static StreamWriter Writer;

    public async Task Connect(string ip, int port)
    {
        try
        {
            Socket = new TcpClient();

            await Socket.ConnectAsync(ip, port);

            if (Socket.Connected)
            {
                Console.WriteLine("Connected to server");

                Stream = new SslStream(
                    Socket.GetStream(),
                    false,
                    (sender, certificate, chain, sslPolicyErrors) => true //disable on release
                );

                await Stream.AuthenticateAsClientAsync(ip);

                Reader = new StreamReader(Stream);
                Writer = new StreamWriter(Stream)
                {
                    AutoFlush = true
                };

                _ = PacketService.PacketReader(Reader);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to connect to server");
        }
    }
}