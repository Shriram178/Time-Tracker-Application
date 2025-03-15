namespace TimeTracker.Tests;

[Collection("SequentialTests")]
public class FileHandlerTests : IDisposable
{
    private readonly FileHandler _fileHandler;
    private readonly string _testBaseDirectory;

    public FileHandlerTests()
    {
        _fileHandler = new FileHandler();
        _testBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeTrackingApp");

        if (Directory.Exists(_testBaseDirectory))
        {
            Directory.Delete(_testBaseDirectory, true);
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(_testBaseDirectory))
        {
            Directory.Delete(_testBaseDirectory, true);
        }
    }

    [Fact]
    public void DirectoryDoesNotExist_EnsureDirectoryExists_CreatesDirectory()
    {
        string testPath = Path.Combine(_testBaseDirectory, "TestDirectory");

        _fileHandler.EnsureDirectoryExists(testPath);

        Assert.True(Directory.Exists(testPath));
    }

    [Fact]
    public void GetUsersFilePath_ReturnsCorrectPath()
    {
        string usersFilePath = _fileHandler.GetUsersFilePath();

        Assert.Equal(Path.Combine(_testBaseDirectory, "Users.csv"), usersFilePath);
    }

    [Fact]
    public void GetProjectsFilePath_ReturnsCorrectPath()
    {
        string projectsFilePath = _fileHandler.GetProjectsFilePath();

        Assert.Equal(Path.Combine(_testBaseDirectory, "Projects.csv"), projectsFilePath);
    }

    [Fact]
    public void CreateProjectFolder_CreatesProjectDirectory()
    {
        string username = "testuser";
        string projectName = "TestProject";

        _fileHandler.CreateProjectFolder(username, projectName);

        string projectPath = Path.Combine(_testBaseDirectory, "Users", username, "Project_" + projectName);
        Assert.True(Directory.Exists(projectPath));
    }

    [Fact]
    public void GetProjectFolders_ReturnsProjectDirectories()
    {
        string username = "testuser";
        string projectName1 = "TestProject1";
        string projectName2 = "TestProject2";
        _fileHandler.CreateProjectFolder(username, projectName1);
        _fileHandler.CreateProjectFolder(username, projectName2);

        List<string> projectFolders = _fileHandler.GetProjectFolders(username);

        Assert.Contains(Path.Combine(_testBaseDirectory, "Users", username, "Project_" + projectName1), projectFolders);
        Assert.Contains(Path.Combine(_testBaseDirectory, "Users", username, "Project_" + projectName2), projectFolders);
    }

    [Fact]
    public void CreateTaskFolder_CreatesTaskDirectory()
    {
        string username = "testuser";
        string projectName = "TestProject";
        string taskName = "TestTask";
        _fileHandler.CreateProjectFolder(username, projectName);

        _fileHandler.CreateTaskFolder(username, projectName, taskName);

        string taskPath = Path.Combine(_testBaseDirectory, "Users", username, projectName, "Task_" + taskName);
        Assert.True(Directory.Exists(taskPath));
    }

    [Fact]
    public void GetTaskFolders_ReturnsTaskDirectories()
    {

        string username = "testuser";
        string projectName = "TestProject";
        string taskName1 = "TestTask1";
        string taskName2 = "TestTask2";
        _fileHandler.CreateProjectFolder(username, projectName);
        _fileHandler.CreateTaskFolder(username, projectName, taskName1);
        _fileHandler.CreateTaskFolder(username, projectName, taskName2);

        List<string> taskFolders = _fileHandler.GetTaskFolders(username, projectName);

        Assert.Contains(Path.Combine(_testBaseDirectory, "Users", username, projectName, "Task_" + taskName1), taskFolders);
        Assert.Contains(Path.Combine(_testBaseDirectory, "Users", username, projectName, "Task_" + taskName2), taskFolders);
    }

    [Fact]
    public void CreateSubTaskFolder_CreatesSubTaskDirectory()
    {

        string username = "testuser";
        string projectName = "TestProject";
        string taskName = "TestTask";
        string subTaskName = "TestSubTask";
        _fileHandler.CreateProjectFolder(username, projectName);
        _fileHandler.CreateTaskFolder(username, projectName, taskName);

        _fileHandler.CreateSubTaskFolder(username, projectName, taskName, subTaskName);

        string subTaskPath = Path.Combine(_testBaseDirectory, "Users", username, projectName, taskName, "SubTask_" + subTaskName);
        Assert.True(Directory.Exists(subTaskPath));
    }

    [Fact]
    public void GetSubTaskFolders_ReturnsSubTaskDirectories()
    {

        string username = "testuser";
        string projectName = "TestProject";
        string taskName = "TestTask";
        string subTaskName1 = "TestSubTask1";
        string subTaskName2 = "TestSubTask2";
        _fileHandler.CreateProjectFolder(username, projectName);
        _fileHandler.CreateTaskFolder(username, projectName, taskName);
        _fileHandler.CreateSubTaskFolder(username, projectName, taskName, subTaskName1);
        _fileHandler.CreateSubTaskFolder(username, projectName, taskName, subTaskName2);

        List<string> subTaskFolders = _fileHandler.GetSubTaskFolders(username, projectName, taskName);

        Assert.Contains(Path.Combine(_testBaseDirectory, "Users", username, projectName, taskName, "SubTask_" + subTaskName1), subTaskFolders);
        Assert.Contains(Path.Combine(_testBaseDirectory, "Users", username, projectName, taskName, "SubTask_" + subTaskName2), subTaskFolders);
    }

    [Fact]
    public void UserHasNoProjects_GetProjectFolders_ReturnsEmptyList()
    {
        string username = "testuser";

        List<string> projectFolders = _fileHandler.GetProjectFolders(username);

        Assert.Empty(projectFolders);
    }

    [Fact]
    public void UserHasNoTasks_GetTaskFolders_ReturnsEmptyList()
    {

        string username = "testuser";
        string projectName = "TestProject";
        _fileHandler.CreateProjectFolder(username, projectName);

        List<string> taskFolders = _fileHandler.GetTaskFolders(username, projectName);

        Assert.Empty(taskFolders);
    }

    [Fact]
    public void UserHasNoSubTasks_GetSubTaskFolders_ReturnsEmptyList()
    {
        string username = "testuser";
        string projectName = "TestProject";
        string taskName = "TestTask";
        _fileHandler.CreateProjectFolder(username, projectName);
        _fileHandler.CreateTaskFolder(username, projectName, taskName);

        List<string> subTaskFolders = _fileHandler.GetSubTaskFolders(username, projectName, taskName);

        Assert.Empty(subTaskFolders);
    }

    [Fact]
    public void UserHasDetailedTasks_GetDetailedTaskInfo_ReturnsTaskInfoDictionary()
    {

        string username = "testuser";
        string projectName = "TestProject";
        string taskName = "TestTask";
        string subTaskName = "TestSubTask";
        _fileHandler.CreateProjectFolder(username, projectName);
        _fileHandler.CreateTaskFolder(username, projectName, taskName);
        _fileHandler.CreateSubTaskFolder(username, projectName, taskName, subTaskName);
        string subTaskDir = Path.Combine(_testBaseDirectory, "Users", username, projectName, taskName, "SubTask_" + subTaskName);
        string timeEntryFile = Path.Combine(subTaskDir, "TimeEntry.csv");
        File.WriteAllLines(timeEntryFile, new[] { "2025-03-15 08:00:00,2025-03-15 09:00:00,Description" });

        var result = _fileHandler.GetDetailedTaskInfo(username);

        Assert.NotEmpty(result);
        Assert.Contains("TestProject", result);
    }

    [Fact]
    public void UserHasNoDetailedTasks_GetDetailedTaskInfo_ReturnsEmptyDictionary()
    {
        string username = "testuser";

        var result = _fileHandler.GetDetailedTaskInfo(username);

        Assert.Empty(result);
    }
}

