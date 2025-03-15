using System.Diagnostics;

namespace TimeTracker;

/// <summary>
/// Manages time tracking operations, including starting, stopping, and displaying timers.
/// </summary>
public class TimeTrackingManager
{
    private readonly FileHandler _fileHandler;
    private Stopwatch _stopwatch;
    private string _currentFilePath;
    private string _task;
    private bool _isBillable;
    private bool _isRunning;
    private bool _pauseTimerDisplay;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeTrackingManager"/> class.
    /// </summary>
    /// <param name="fileHandler">The file handler for managing file operations.</param>
    public TimeTrackingManager(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
        _stopwatch = new Stopwatch();
    }

    /// <summary>
    /// Starts the timer for a specified task and subtask.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="project">The name of the project.</param>
    /// <param name="task">The name of the task.</param>
    /// <param name="subtask">The name of the subtask.</param>
    /// <param name="isBillable">Indicates whether the task is billable.</param>
    public void StartTimer(
        string username,
        string project,
        string task,
        string subtask,
        bool isBillable)
    {
        if (_stopwatch.IsRunning)
        {
            Console.WriteLine("A timer is already running. Stop it first!");
            Thread.Sleep(1500);
            return;
        }

        string subTaskPath = Path.Combine(
            _fileHandler.GetProjectsFilePath(),
            "Users", username, project, task, subtask);

        Directory.CreateDirectory(subTaskPath);
        _currentFilePath = Path.Combine(subTaskPath, "TimeEntry.csv");

        _task = Path.GetFileName(subTaskPath);
        _isBillable = isBillable;
        _stopwatch.Restart();
        _isRunning = true;
        _pauseTimerDisplay = false;

        Console.Clear();
        Console.SetCursorPosition(0, 1);
        Console.WriteLine("Timer started...");

        Thread timerThread = new Thread(DisplayTimer);
        timerThread.Start();
    }

    /// <summary>
    /// Pauses or resumes the timer display.
    /// </summary>
    /// <param name="pause">Indicates whether to pause the timer display.</param>
    public void PauseTimerDisplay(bool pause)
    {
        _pauseTimerDisplay = pause;
    }

    /// <summary>
    /// Displays the timer on the console.
    /// </summary>
    private void DisplayTimer()
    {
        while (_isRunning)
        {
            if (_pauseTimerDisplay)
            {
                Thread.Sleep(500);
                continue;
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.Write($"Working on {_task} - Elapsed time: {_stopwatch.Elapsed:hh\\:mm\\:ss}\n\n");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        // Clear the timer when stopping
        Console.SetCursorPosition(0, 0);
        Console.Write(new string(' ', Console.WindowWidth));
    }

    /// <summary>
    /// Stops the currently running timer and logs the time entry.
    /// </summary>
    public void StopTimer()
    {
        if (!_stopwatch.IsRunning)
        {
            Console.WriteLine("No active timer to stop.");
            return;
        }

        _stopwatch.Stop();
        _isRunning = false;
        TimeSpan duration = _stopwatch.Elapsed;
        string startTime = DateTime.Now.Subtract(duration).ToString("yyyy-MM-dd HH:mm:ss");
        string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        File.AppendAllText(_currentFilePath, $"{startTime},{endTime},{(_isBillable ? "Yes" : "No")}\n");

        Console.SetCursorPosition(0, 0);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, 2);

        Console.WriteLine($"\nTimer stopped. Duration: {duration}");
        Thread.Sleep(1500);
    }

    /// <summary>
    /// Stops the currently running timer and logs the time entry.
    /// </summary>
    public string GetUserInput(string prompt)
    {
        _pauseTimerDisplay = true; // Pause timer display to avoid overlap
        Console.SetCursorPosition(0, Console.CursorTop + 1);
        Console.Write(prompt);
        string input = Console.ReadLine();
        _pauseTimerDisplay = false;
        return input;
    }
}

