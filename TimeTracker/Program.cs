namespace TimeTracker;

/// <summary>
/// The main entry point for the Taskify.
/// </summary>
public class Program
{
    /// <summary>
    /// The main method that initializes and runs the application.
    /// </summary>
    static void Main()
    {
        Console.WindowWidth = 190;
        Console.WindowHeight = 40;
        FileHandler fileHandler = new FileHandler();
        Logger logger = new Logger();
        UserManager userManager = new UserManager(fileHandler);
        TimeTrackingManager timeTrackingManager = new TimeTrackingManager(fileHandler);
        UserInteractor userInteractor = new UserInteractor(userManager, logger, fileHandler, timeTrackingManager);

        RunApplication(userInteractor, logger);
    }

    /// <summary>
    /// Runs the main application loop, displaying the menu and handling user choices.
    /// </summary>
    /// <param name="userInteractor">The user interactor for handling user interactions.</param>
    /// <param name="logger">The logger for displaying messages.</param>
    static void RunApplication(UserInteractor userInteractor, Logger logger)
    {
        bool isRunning = true;
        while (isRunning)
        {
            try
            {
                Console.Clear();
                string choice = userInteractor.DisplayMenu();

                switch (choice)
                {
                    case "Register":
                        userInteractor.Register();
                        break;
                    case "Login":
                        userInteractor.Login();
                        break;
                    case "Exit":
                        isRunning = false;
                        break;
                    default:
                        logger.DisplayFailure("Invalid choice! Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.DisplayFailure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

}
