namespace Chatinator.Client.Helpers;

public static class ConsoleHelpers
{
    public static bool Query(string message)
    {
        Console.WriteLine(message + "(y/n)");
        while (true)
        {
            var input = Console.ReadLine() ?? "";
            if (input == "y")
                return true;
            if (input == "n")
                return false;
            Console.WriteLine("Invalid input.");
        }
    }
}