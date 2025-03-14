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
    /// Gets the list of projects the user has worked on today based on the folder's last modified date.
    /// </summary>
    public List<string> GetRecentProjects(string username)
    {
        string userPath = Path.Combine(BaseDirectory, "Users", username);
        if (!Directory.Exists(userPath))
            return new List<string>();

        DateTime today = DateTime.Today;
        return Directory.GetDirectories(userPath)
            .Select(dir => new DirectoryInfo(dir))
            .Where(dirInfo => dirInfo.LastWriteTime.Date == today)
            .OrderByDescending(dirInfo => dirInfo.LastWriteTime)
            .Select(dirInfo => dirInfo.Name)
            .ToList();
    }

}
