namespace TimeTracker;

public class FileHandler
{
    private readonly string BaseDirectory;

    public FileHandler()
    {
        BaseDirectory = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData)
            , "TimeTrackingApp");

        EnsureDirectoryExists(BaseDirectory);
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public string GetUsersFilePath() => Path.Combine(BaseDirectory, "Users.csv");
    public string GetProjectsFilePath() => Path.Combine(BaseDirectory, "Projects.csv");

    public void CreateProjectFolder(string username, string projectName)
    {
        string projectPath = Path.Combine(BaseDirectory, "Users", username, "Project_" + projectName);
        EnsureDirectoryExists(projectPath);
    }

    public List<string> GetProjectFolders(string username)
    {
        string userPath = Path.Combine(BaseDirectory, "Users", username);
        return Directory.Exists(userPath) ? new List<string>(Directory.GetDirectories(userPath)) : new List<string>();
    }

    public void CreateTaskFolder(string username, string projectName, string taskName)
    {
        string taskPath = Path.Combine(BaseDirectory, "Users", username, projectName, "Task_" + taskName);
        EnsureDirectoryExists(taskPath);
    }

    public List<string> GetTaskFolders(string username, string projectName)
    {
        string projectPath = Path.Combine(BaseDirectory, "Users", username, projectName);
        return Directory.Exists(projectPath) ? new List<string>(Directory.GetDirectories(projectPath)) : new List<string>();
    }

    public void CreateSubTaskFolder(string username, string projectName, string taskName, string subTaskName)
    {
        string subTaskPath = Path.Combine(BaseDirectory, "Users", username, projectName, taskName, "SubTask_" + subTaskName);
        EnsureDirectoryExists(subTaskPath);
    }

    public List<string> GetSubTaskFolders(string username, string projectName, string taskName)
    {
        string taskPath = Path.Combine(BaseDirectory, "Users", username, projectName, taskName);
        return Directory.Exists(taskPath) ? new List<string>(Directory.GetDirectories(taskPath)) : new List<string>();
    }

    /// <summary>
    /// Gathers detailed information about tasks and subtasks, including their durations and descriptions.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>A dictionary containing detailed task information.</returns>
    public Dictionary<string, Dictionary<string, Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>>>> GetDetailedTaskInfo(string username)
    {
        string userPath = Path.Combine(BaseDirectory, "Users", username);
        if (!Directory.Exists(userPath))
            return new Dictionary<string, Dictionary<string, Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>>>>();

        DateTime today = DateTime.Today;
        var detailedTaskInfo = new Dictionary<string, Dictionary<string, Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>>>>();

        foreach (var projectDir in Directory.GetDirectories(userPath))
        {
            DirectoryInfo projectInfo = new DirectoryInfo(projectDir);

            string projectName = projectInfo.Name;
            var projectTasks = GetProjectTasks(projectDir, today);

            if (projectTasks.Any())
            {
                detailedTaskInfo[projectName] = projectTasks;
            }
        }

        return detailedTaskInfo;
    }

    public void ExportRecentWorkToFile(string userName, int count = -1)
    {
        string filePath = Path.Combine(BaseDirectory, "Users", userName, "RecentWork.txt");
        var detailedTaskInfo = GetDetailedTaskInfo(userName);

        if (!detailedTaskInfo.Any())
        {
            Console.WriteLine("[-] No recent work entries found for today.");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            string numberOfEntries = count == -1 ? "" : $"{count}";
            writer.WriteLine($"\n{numberOfEntries} Recent Work's performed Today:");
            writer.WriteLine("───────────────────────────────────────────");

            int iterations = 0;
            foreach (var project in detailedTaskInfo)
            {
                if (iterations++ == count) break;

                TimeSpan projectDuration = project.Value.Values.SelectMany(
                    task => task.Values.SelectMany(
                        subtask => subtask.Select(
                            entry => entry.EndTime - entry.StartTime)))
                    .Aggregate(TimeSpan.Zero, (sum, duration) => sum.Add(duration));

                writer.WriteLine($"\n{project.Key} - {projectDuration}");

                foreach (var task in project.Value)
                {
                    TimeSpan taskDuration = task.Value.Values.SelectMany(
                        subtask => subtask.Select(
                            entry => entry.EndTime - entry.StartTime))
                        .Aggregate(TimeSpan.Zero, (sum, duration) => sum.Add(duration));

                    writer.WriteLine($"  {task.Key} - {taskDuration}");

                    foreach (var subtask in task.Value)
                    {
                        TimeSpan subtaskDuration = subtask.Value.Select(
                            entry => entry.EndTime - entry.StartTime)
                            .Aggregate(TimeSpan.Zero, (sum, duration) => sum.Add(duration));

                        writer.WriteLine($"    {subtask.Key} - {subtaskDuration}");

                        foreach (var entry in subtask.Value)
                        {
                            writer.WriteLine($"      -  {entry.StartTime:HH:mm:ss} - {entry.EndTime:HH:mm:ss}");
                        }
                    }
                }
            }

            writer.WriteLine("───────────────────────────────────────────\n");
        }
    }

    /// <summary>
    /// Retrieves tasks for a given project directory.
    /// </summary>
    /// <param name="projectDir">The project directory path.</param>
    /// <param name="today">The current date.</param>
    /// <returns>A dictionary containing task information.</returns>
    private Dictionary<string, Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>>> GetProjectTasks(string projectDir, DateTime today)
    {
        var projectTasks = new Dictionary<string, Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>>>();

        foreach (var taskDir in Directory.GetDirectories(projectDir))
        {
            string taskName = Path.GetFileName(taskDir);
            var taskSubtasks = GetTaskSubtasks(taskDir, today);

            if (taskSubtasks.Any())
            {
                projectTasks[taskName] = taskSubtasks;
            }
        }

        return projectTasks;
    }

    /// <summary>
    /// Retrieves subtasks for a given task directory.
    /// </summary>
    /// <param name="taskDir">The task directory path.</param>
    /// <param name="today">The current date.</param>
    /// <returns>A dictionary containing subtask information.</returns>
    private Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>> GetTaskSubtasks(string taskDir, DateTime today)
    {
        var taskSubtasks = new Dictionary<string, List<(string Description, DateTime StartTime, DateTime EndTime)>>();

        foreach (var subtaskDir in Directory.GetDirectories(taskDir))
        {
            string subtaskName = Path.GetFileName(subtaskDir);
            var subtaskEntries = GetSubtaskEntries(subtaskDir, today);

            if (subtaskEntries.Any())
            {
                taskSubtasks[subtaskName] = subtaskEntries;
            }
        }

        return taskSubtasks;
    }

    /// <summary>
    /// Retrieves time entries for a given subtask directory.
    /// </summary>
    /// <param name="subtaskDir">The subtask directory path.</param>
    /// <param name="today">The current date.</param>
    /// <returns>A list of time entries for the subtask.</returns>
    private List<(string Description, DateTime StartTime, DateTime EndTime)> GetSubtaskEntries(string subtaskDir, DateTime today)
    {
        var subtaskEntries = new List<(string Description, DateTime StartTime, DateTime EndTime)>();

        string timeEntryFile = Path.Combine(subtaskDir, "TimeEntry.csv");
        if (File.Exists(timeEntryFile))
        {
            var lines = File.ReadAllLines(timeEntryFile);
            foreach (var line in lines)
            {
                var data = line.Split(',');
                if (data.Length == 3 && DateTime.TryParse(data[0], out DateTime startTime) && DateTime.TryParse(data[1], out DateTime endTime))
                {
                    if (startTime.Date == today)
                    {
                        subtaskEntries.Add((data[2], startTime, endTime));
                    }
                }
            }
        }

        return subtaskEntries;
    }

}
