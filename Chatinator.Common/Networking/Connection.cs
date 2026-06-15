using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Chatinator.Common.Networking;

/// <summary>The Connection class is a class that represents a connection between a client and the server. </summary>
public class Connection : IDisposable
{


    /// <summary>The Socket property is the TcpClient object used to communicate with the client.</summary>
    public TcpClient Socket { get; private set; }
    /// <summary>The Stream property represents the encrypted stream used to send and receive data over the connection</summary>
    public SslStream Stream { get; }
    /// <summary>The Reader property is the StreamReader object used to read data from the connection</summary>
    public StreamReader Reader { get; private set; }
    /// <summary>The Writer property is the StreamWriter object used to write data to the connection.
    /// AutoFlush is set to true so that the data is automatically flushed to the stream, therefore sending the packet</summary>
    public StreamWriter Writer { get; private set; }
    
    
    public Connection(TcpClient socket, SslStream stream)
    {
        Socket = socket;
        Stream = stream;
        Reader = new StreamReader(Stream);
        Writer = new StreamWriter(Stream)
        {
            AutoFlush = true
        };
        
    }
    
    public void Dispose()
    {
        Socket.Close();
        Stream.Dispose();
        Reader.Dispose();
        Writer.Dispose();
    }
}

/// <summary>
/// The ConnectionFactory is a class for creating various connection objects.
/// Server connections are for when a client connects. Every time CreateServerConnectionAsync() is run, it returns a new Connection Object
/// Client connections are for when a client wants to connect to the server. It also returns a new Connection Object.
/// </summary>
public class ConnectionFactory
{
    
    /// <summary>used to make a connection to the client from the server.</summary>
    /// <returns>Connection</returns>
    public static async Task<Connection> CreateServerConnectionAsync(TcpClient socket, X509Certificate2 certificate)
    {
        // create a new encrypted stream out of the client's connection
        var stream = new SslStream(socket.GetStream(), false);

        // authenticate the client with the server certificate
        await stream.AuthenticateAsServerAsync(certificate);

        // return a new connection object
        return new Connection(socket, stream);
    }

    /// <summary>Used to connect to the server.</summary>
    /// <returns>Connection</returns>
    public static async Task<Connection> CreateClientConnectionAsync(string host, int port)
    {
        // create a new socket used to connect to the server
        var socket = new TcpClient();

        // connect to the server
        await socket.ConnectAsync(host, port);

        // create a new encrypted stream out of the client's connection
        var stream = new SslStream(socket.GetStream());
        
        // authenticate the client with the server certificate
        await stream.AuthenticateAsClientAsync(host);
        


        // return a new connection object
        return new Connection(socket, stream);
    }
}



/// <summary>
/// The Connection Server handles multiple tasks:
/// Load the X509 Certificate.
/// Listen for incoming connections.
/// Encrypt the connection using TLS.
/// Handle incoming connections with a delegate.
/// </summary>
public class ConnectionServer
{
    private readonly X509Certificate2 _cert = X509Certificate2.CreateFromEncryptedPemFile(
        "./Data/server.pfx", "password123");

    // a dictionary of all the connections that are currently connected to the server. 
    // do not add the connection to this dictionary until the client has been authenticated and a GUID has been either assigned or obtained from the client or database.
    public static Dictionary<Guid, Connection> Connections = new();
    
    private TcpListener _listener;


    public async Task ListenAsync(int port, Func<Connection, Task> handler)
    {
        // create a new listener that the server will use to listen for incoming connections
        _listener = new TcpListener(IPAddress.Any, port);

        // start listening for incoming connections
        _listener.Start();

        // loop forever and wait for incoming connections and handle them with a delegate. because of this, this should be run in a background thread so the method can return.
        _ = Task.Run(async () =>
        {
            while (true)
            {
                // accept an incoming connection
                var tcpClient = await _listener.AcceptTcpClientAsync();
                
                // try to create a new connection object from the client
                // if it fails, the client is not authenticated and will be disconnected.
                Connection? connection = null;
                try
                {
                    connection = await ConnectionFactory.CreateServerConnectionAsync(tcpClient, _cert);
                }
                catch (Exception e)
                {
                    // if the client is not authenticated, close the connection
                    // check the exception type to see if the client is not authenticated
                    if (e is AuthenticationException)
                    {
                        Console.WriteLine("Authentication failure. Closing connection.");
                        tcpClient.Close();
                    }
                    else
                    {
                        Console.WriteLine("An error occurred while creating a connection:");
                        Console.WriteLine(e);
                        tcpClient.Close();
                    }
                    
                    connection?.Dispose();
                    
                    // continue to the next connection so that the server does not handle a connection that has failed.
                    continue;
                }

                _ = handler(connection);
            }
        });
    }
    
    
}