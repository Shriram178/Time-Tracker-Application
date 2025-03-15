using TimeTracker.Model;

namespace TimeTracker.Tests;

[Collection("SequentialTests")]
public class UserManagerTests
{
    private readonly UserManager _userManager;
    private readonly FileHandler _fileHandler;
    private readonly string _testBaseDirectory;

    public UserManagerTests()
    {
        _fileHandler = new FileHandler();
        _userManager = new UserManager(_fileHandler);
        _testBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeTrackingApp");

        Directory.CreateDirectory(_testBaseDirectory);

        var usersFilePath = _fileHandler.GetUsersFilePath();
        if (!File.Exists(usersFilePath))
        {
            File.Create(usersFilePath).Dispose();
        }
    }

    public void Dispose()
    {
        // Clean up test environment
        if (Directory.Exists(_testBaseDirectory))
        {
            Directory.Delete(_testBaseDirectory, true);
        }
    }

    [Fact]
    public void UserDoesNotExist_RegisterUser_UserIsAdded()
    {
        var user = new User("testuser", "password", false);

        bool result = _userManager.RegisterUser(user);

        Assert.NotNull(_userManager.GetUser("testuser"));
    }

    [Fact]
    public void UserAlreadyExists_RegisterUser_UserIsNotAdded()
    {
        var user = new User("testuser", "password", false);
        _userManager.RegisterUser(user);

        var result = _userManager.RegisterUser(user);

        Assert.False(result);
    }

    [Fact]
    public void ValidCredentials_IsLoginValid_ReturnsTrue()
    {
        var user = new User("testuser", "password", false);
        _userManager.RegisterUser(user);

        var result = _userManager.IsLoginValid("testuser", "password");

        Assert.True(result);
    }

    [Fact]
    public void InvalidCredentials_IsLoginValid_ReturnsFalse()
    {
        var user = new User("testuser", "password", false);
        _userManager.RegisterUser(user);

        var result = _userManager.IsLoginValid("testuser", "wrongpassword");

        Assert.False(result);
    }

    [Fact]
    public void UserExists_GetUser_ReturnsUser()
    {
        var user = new User("testuser", "password", false);
        _userManager.RegisterUser(user);

        var result = _userManager.GetUser("testuser");

        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
    }

    [Fact]
    public void UserDoesNotExist_GetUser_ReturnsNull()
    {
        var result = _userManager.GetUser("nonexistentuser");

        Assert.Null(result);
    }

    [Fact]
    public void UserExists_IsUserPresent_ReturnsTrue()
    {
        var user = new User("testuser", "password", false);
        _userManager.RegisterUser(user);

        var result = _userManager.IsUserPresent("testuser");

        Assert.True(result);
    }

    [Fact]
    public void UserDoesNotExist_IsUserPresent_ReturnsFalse()
    {
        var result = _userManager.IsUserPresent("nonexistentuser");

        Assert.False(result);
    }
}