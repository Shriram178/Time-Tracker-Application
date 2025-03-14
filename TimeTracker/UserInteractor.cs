using TimeTracker.Constants;
using TimeTracker.Model;

namespace TimeTracker;

/// <summary>
/// This is the class with which the user
/// interacts to perform operations
/// </summary>
public class UserInteractor
{
    private readonly UserManager _userManager;
    private readonly Logger _logger;
    private readonly FileHandler _fileHandler;
    private readonly TimeTrackingManager _timeTrackingManager;
    private User? _loggedInUser;

    /// <summary>
    /// Constructor to perform dependency injection.
    /// </summary>
    public UserInteractor(UserManager userManager, Logger logger, FileHandler fileHandler, TimeTrackingManager timeTrackingManager)
    {
        _userManager = userManager;
        _logger = logger;
        _fileHandler = fileHandler;
        _timeTrackingManager = timeTrackingManager;
    }

    /// <summary>
    /// Displays the starting menu of the app
    /// while also getting the user choice.
    /// </summary>
    /// <returns>userChoice as integer</returns>
    public string DisplayMenu()
    {

        _logger.DisplayTitle(StringConstants.Greetings);
        string? UserChoice = CreateDropDown<string>(StringConstants.Authorization, StringConstants.Greetings, "");
        return UserChoice;
    }

    /// <summary>
    /// Prompts the user to get info about details to
    /// register the user then send it to <see cref="UserManager"/>
    /// </summary>
    public void Register()
    {
        string userName = PromptForInput("Enter the username: ", "Username cannot be empty!");

        if (_userManager.IsUserPresent(userName))
        {
            Console.Clear();
            _logger.DisplayFailure($"{userName} is already taken! Try another username.");
            return;
        }

        string userPassword = GetUserPassword();

        // Ask if the user is a manager (Dropdown choice)
        string roleChoice = CreateDropDown(StringConstants.UserRoles,
                                           "Select User Role:",
                                           "[Up/Down] to navigate, [Enter] to select");

        bool isManager = roleChoice == "Manager"; // If they chose "Manager", set isManager to true.

        // Create and register the user
        User user = new User(userName, userPassword, isManager);
        _userManager.RegisterUser(user);

        Console.Clear();
        _logger.DisplaySuccess($"User {userName} created successfully as {(isManager ? "Manager" : "Regular User")}!");
    }

    /// <summary>
    /// Prompts the user for login credentials and then sends it to
    /// <see cref="UserManager.IsLoginValid(string, string)"/> to Log the user in.
    /// </summary>
    public void Login()
    {
        string userName = PromptForInput("Enter the username: ", "Username cannot be empty!");
        if (_userManager.IsUserPresent(userName))
        {
            string userPassword = GetUserPassword();
            if (_userManager.IsLoginValid(userName, userPassword))
            {
                _loggedInUser = _userManager.GetUser(userName); // Store the User object
                _logger.DisplaySuccess($"Logged in as {_loggedInUser.UserName}!");
                ShowDashboard();
            }
            else
            {
                _logger.DisplayFailure("\nWrong password! Try again.");
            }
        }
        else
        {
            _logger.DisplayFailure($"No user with name {userName}.");
        }
    }

    /// <summary>
    /// Displays the dashboard where users can create projects, tasks, subtasks, and start timers.
    /// </summary>
    /// <summary>
    /// Displays the dashboard where users can create projects, tasks, subtasks, and start timers.
    /// </summary>
    public void ShowDashboard()
    {
        if (_loggedInUser == null)
        {
            _logger.DisplayFailure("No user logged in!");
            return;
        }

        while (true)
        {
            List<string> projectPaths = _fileHandler.GetProjectFolders(_loggedInUser.UserName);
            Dictionary<string, string> projectMap = projectPaths.ToDictionary(p => Path.GetFileName(p), p => p);

            List<string> options = new();
            options.AddRange(projectMap.Keys);
            options.Add("Create Project");
            options.Add("View Recent Work");



            string choice = CreateDropDown(options, $"Projects - {_loggedInUser.UserName}", "[Up/Down] to navigate, [Enter] to select, [Esc] to exit");

            if (choice == null) return; // Exit on ESC
            if (choice == "Create Project")
            {
                CreateProject(_loggedInUser.UserName);
            }
            else if (choice == "View Recent Work")
            {
                DisplayRecentWork();
                Console.ReadLine(); // Pause to allow user to read the recent work
            }
            else
            {
                ShowTaskMenu(_loggedInUser.UserName, projectMap[choice]); // Pass full path
            }
        }
    }

    private void DisplayRecentWork()
    {
        if (_loggedInUser == null) return;

        // Get detailed task information directly from FileHandler
        var detailedTaskInfo = _fileHandler.GetDetailedTaskInfo(_loggedInUser.UserName);

        if (!detailedTaskInfo.Any())
        {
            _logger.DisplayFailure("No recent work entries found for today.");
            return;
        }

        Console.WriteLine("\nRecent Work Today:");
        Console.WriteLine("───────────────────────────────────────────");

        foreach (var project in detailedTaskInfo)
        {
            TimeSpan projectDuration = project.Value.Values.SelectMany(
                task => task.Values.SelectMany(
                    subtask => subtask.Select(
                        entry => entry.EndTime - entry.StartTime)))
                .Aggregate(TimeSpan.Zero, (sum, duration) => sum.Add(duration));

            Console.WriteLine($"\n{project.Key} - {projectDuration}");

            foreach (var task in project.Value)
            {
                TimeSpan taskDuration = task.Value.Values.SelectMany(
                    subtask => subtask.Select(
                        entry => entry.EndTime - entry.StartTime))
                    .Aggregate(TimeSpan.Zero, (sum, duration) => sum.Add(duration));

                Console.WriteLine($"  {task.Key} - {taskDuration}");

                foreach (var subtask in task.Value)
                {
                    TimeSpan subtaskDuration = subtask.Value.Select(
                        entry => entry.EndTime - entry.StartTime)
                        .Aggregate(TimeSpan.Zero, (sum, duration) => sum.Add(duration));

                    Console.WriteLine($"    {subtask.Key} - {subtaskDuration}");

                    foreach (var entry in subtask.Value)
                    {
                        Console.WriteLine($"      -  {entry.StartTime:HH:mm:ss} - {entry.EndTime:HH:mm:ss}");
                    }
                }
            }
        }

        Console.WriteLine("───────────────────────────────────────────\n");
    }




    private void ShowTaskMenu(string username, string projectPath)
    {
        while (true)
        {
            List<string> taskPaths = _fileHandler.GetTaskFolders(username, projectPath);
            Dictionary<string, string> taskMap = taskPaths.ToDictionary(t => Path.GetFileName(t), t => t);

            List<string> options = new() { "Create Task" };
            options.AddRange(taskMap.Keys);

            string choice = CreateDropDown(options, $"Tasks in {Path.GetFileName(projectPath)}", "[Up/Down] to navigate, [Enter] to select, [Esc] to go back");

            if (choice == null) return; // Back on ESC
            if (choice == "Create Task")
            {
                CreateTask(username, projectPath);
            }
            else
            {
                ShowSubtaskMenu(username, projectPath, taskMap[choice]); // Pass full path
            }
        }
    }

    private void ShowSubtaskMenu(string username, string projectPath, string taskPath)
    {
        while (true)
        {
            List<string> subtaskPaths = _fileHandler.GetSubTaskFolders(username, projectPath, taskPath);
            Dictionary<string, string> subtaskMap = subtaskPaths.ToDictionary(st => Path.GetFileName(st), st => st);

            List<string> options = new() { "Create Subtask" };
            options.AddRange(subtaskMap.Keys);

            string choice = CreateDropDown(options, $"Subtasks in {Path.GetFileName(taskPath)}", "[Up/Down] to navigate, [Enter] to select, [Esc] to go back");

            if (choice == null) return; // Back on ESC
            if (choice == "Create Subtask")
            {
                CreateSubtask(username, projectPath, taskPath);
            }
            else
            {
                ShowTimerMenu(username, projectPath, taskPath, subtaskMap[choice]); // Pass full path
            }
        }
    }

    private void ShowTimerMenu(string username, string project, string task, string subtask)
    {
        while (true)
        {
            List<string> options = new() { "Start Timer", "Stop Timer" };

            string choice = CreateDropDown(options, $"Timer for {subtask}", "[Up/Down] to navigate, [Enter] to select, [Esc] to go back");

            if (choice == null) return; // Back on ESC
            if (choice == "Start Timer")
            {
                bool isBillable = CreateDropDown(new List<string> { "Yes", "No" }, "Is this work billable?", "[Up/Down] to navigate, [Enter] to select") == "Yes";
                _timeTrackingManager.StartTimer(username, project, task, subtask, isBillable);
                _logger.DisplaySuccess("Timer started for subtask!");
            }
            else if (choice == "Stop Timer")
            {
                _timeTrackingManager.StopTimer();
                _logger.DisplaySuccess("Timer stopped!");
            }
        }
    }

    private void CreateProject(string username)
    {
        string projectName = PromptForInput("Enter project name: ", "Project name cannot be empty!");
        _fileHandler.CreateProjectFolder(username, projectName);
        _logger.DisplaySuccess($"Project '{projectName}' created!");
    }

    private void CreateTask(string username, string project)
    {
        string taskName = PromptForInput("Enter task name: ", "Task name cannot be empty!");
        _fileHandler.CreateTaskFolder(username, project, taskName);
        _logger.DisplaySuccess($"Task '{taskName}' created in '{project}'!");
    }

    private void CreateSubtask(string username, string project, string task)
    {
        string subtaskName = PromptForInput("Enter subtask name: ", "Subtask name cannot be empty!");
        _fileHandler.CreateSubTaskFolder(username, project, task, subtaskName);
        _logger.DisplaySuccess($"Subtask '{subtaskName}' created in '{task}'!");
    }


    private T? CreateDropDown<T>(IList<T> items, string message, string menuOptions)
    {
        int selectedIndex = 0;
        ConsoleKey key;
        while (true)
        {
            Console.WriteLine();
            DrawMenu(items, selectedIndex, message, menuOptions);

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + items.Count) % items.Count;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1 + items.Count) % items.Count;
            }
            else if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                return items[selectedIndex];
            }
            else if (key == ConsoleKey.B || key == ConsoleKey.Escape) // Handle back action
            {
                Console.Clear();
                return default;
            }
        }
    }

    private void DrawMenu<T>(IList<T> categories, int selectedIndex, string message, string menuOptions)
    {
        Console.Clear();
        _logger.DisplayTitle(message);

        // Get the current cursor position after printing the message
        int startRow = Console.CursorTop + 1; // Move below the message

        // Ensure the menu doesn't exceed the window height
        if (startRow + categories.Count >= Console.WindowHeight)
        {
            startRow = Console.WindowHeight - categories.Count - 1;
        }

        // Set cursor to dynamically available top-left position
        Console.SetCursorPosition(0, startRow);

        for (int i = 0; i < categories.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> {categories[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  {categories[i]}");
            }
        }
        _logger.DisplayTitle(menuOptions);
    }

    private string PromptForInput(string prompt, string invalidMessage, string warningMessage = "The string size must be within 20!", int maxLength = 20)
    {
        int inputRow = Console.CursorTop; // Store the input line position

        _timeTrackingManager.PauseTimerDisplay(true); // Pause timer while taking input

        while (true)
        {
            Console.SetCursorPosition(0, inputRow); // Move cursor back to input line
            Console.Write(new string(' ', Console.WindowWidth)); // Clear input line
            Console.SetCursorPosition(0, inputRow);
            Console.Write(prompt); // Rewrite prompt

            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                Console.SetCursorPosition(0, inputRow + 1);
                _logger.DisplayFailure(invalidMessage);
                continue;
            }

            if (input.Length > maxLength)
            {
                Console.SetCursorPosition(0, inputRow + 1);
                _logger.DisplayFailure(warningMessage);
                continue;
            }

            _timeTrackingManager.PauseTimerDisplay(false); // Resume timer after valid input

            return input;
        }
    }

    private string GetUserPassword()
    {
        int inputRow = Console.CursorTop;

        while (true)
        {
            Console.WriteLine();
            Console.SetCursorPosition(0, inputRow); // Move cursor back to input line
            Console.Write(new string(' ', Console.WindowWidth)); // Clear input line
            Console.SetCursorPosition(0, inputRow);
            Console.Write("Enter your password: "); // Rewrite prompt
            string password = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }

            if (string.IsNullOrEmpty(password))
            {
                Console.SetCursorPosition(0, inputRow + 1);
                _logger.DisplayFailure("Password cannot be empty. Please try again.");
            }
            else
            {
                return password;
            }
        }
    }

}
