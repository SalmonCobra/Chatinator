using System.Net.Sockets;
using Chatinator.Client.Helpers;
using Chatinator.Common.Packets;

namespace Chatinator.Client.Authentication;

public static class LoginAuthenticator
{
    public static LoginPacket LoginCli(NetworkStream stream)
    {
        string? username, password;
        var newUser = ConsoleHelpers.Query("New UserModel?"); // new user or existing user query


        if (newUser)
        {
            while (true)
            {
                Console.WriteLine("Please enter a username:");
                username = (Console.ReadLine() ?? "").Trim();
                if (ConsoleHelpers.Query($"Username \"{username}\" Correct?"))
                    break;
            }

            while (true)
            {
                Console.WriteLine("Please enter a password:");
                password = (Console.ReadLine() ?? "").Trim();
                if (ConsoleHelpers.Query($"Password \"{password}\" Correct?"))
                    break;
            }
        }
        else
        {
            Console.WriteLine("Username:");
            username = (Console.ReadLine() ?? "").Trim();
            Console.WriteLine("Password:");
            password = (Console.ReadLine() ?? "").Trim();
        }

        return new LoginPacket { Username = username, Password = password, NewUser = newUser };
    }
}

/* This just asks for a new password
    string password = "";
    while (true)
    {
       Console.WriteLine("Enter a password");
       Console.WriteLine("Rules: No Whitespace, 1 uppercase, 1 lowercase, 1 symbol, 8 characters minimum.");
       password = Console.ReadLine();
       if (Regex.IsMatch(password, @"^(?=.{8,128}$)(?!.*\s)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9\s]).*$"))
       {
           break;
       }
       Console.WriteLine("Invalid password, Please try again.");
    }
 */