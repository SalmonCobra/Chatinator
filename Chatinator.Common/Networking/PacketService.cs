namespace Chatinator.Common.Networking;

public static class PacketService
{
    public static async Task PacketReader(StreamReader reader)
    {
        while (true)
        {
            var message = await reader.ReadLineAsync();
            if (message == null)
                break;

            PacketHandler(message);
        }
    }

    public static async Task<string?> PacketReadNext(StreamReader reader)
    {
        return await reader.ReadLineAsync();
    }


    public static void PacketHandler(string message)
    {
        Console.WriteLine($"server: {message}");
    }

    public static void SendSimplePacket(string text, StreamWriter writer)
    {
        writer.WriteLine(text);
        writer.Flush();
    }
}