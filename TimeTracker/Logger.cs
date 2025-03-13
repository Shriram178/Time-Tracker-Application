namespace TimeTracker;

public class Logger
{
    /// <summary>
    /// Display message in green color
    /// </summary>
    /// <param name="message">string to display</param>
    public void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{message}\n");
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    /// <summary>
    /// Display message in green color
    /// </summary>
    /// <param name="message">string to display</param>
    public void DisplayFailure(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Thread.Sleep(500);
    }

    /// <summary>
    /// Display message in green color
    /// </summary>
    /// <param name="message">string to display</param>
    public void DisplayTitle(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n{message}\n");
        Console.ResetColor();
    }
}
