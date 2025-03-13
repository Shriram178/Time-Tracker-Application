using System.Diagnostics;

namespace TimeTracker;

public class TimeTrackingManager
{
    private readonly FileHandler _fileHandler;
    private Stopwatch _stopwatch;
    private string _currentFilePath;
    private string _workDescription;
    private bool _isBillable;
    private bool _isRunning;

    public TimeTrackingManager(FileHandler fileHandler)
    {
        _fileHandler = fileHandler;
        _stopwatch = new Stopwatch();
    }

    public void StartTimer(string username, string project, string task, string subtask, string workDescription, bool isBillable)
    {
        if (_stopwatch.IsRunning)
        {
            Console.WriteLine("A timer is already running. Stop it first!");
            Thread.Sleep(1500);
            return;
        }

        string subTaskPath = Path.Combine(_fileHandler.GetProjectsFilePath(), "Users", username, project, task, subtask);
        Directory.CreateDirectory(subTaskPath);
        _currentFilePath = Path.Combine(subTaskPath, "TimeEntry.csv");

        _workDescription = workDescription;
        _isBillable = isBillable;
        _stopwatch.Restart();
        _isRunning = true;

        Console.WriteLine("Timer started...");
        Thread timerThread = new Thread(() => DisplayTimer(workDescription));
        timerThread.Start();
    }

    private void DisplayTimer(string workDescription)
    {
        while (_isRunning)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Working on {workDescription} - Elapsed time: {_stopwatch.Elapsed:hh\\:mm\\:ss}");
            Console.ResetColor();
            Thread.Sleep(1000);

        }
    }

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

        File.AppendAllText(_currentFilePath, $"{startTime},{endTime},{_workDescription},{(_isBillable ? "Yes" : "No")}\n");
        Console.WriteLine($"\nTimer stopped. Duration: {duration}");
        Thread.Sleep(1500);
    }
}

