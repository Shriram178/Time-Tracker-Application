namespace TimeTracker;

public class Program
{
    static void Main()
    {
        Console.WindowWidth = 190;
        Console.WindowHeight = 40;
        FileHandler fileHandler = new FileHandler();
        Logger logger = new Logger();
        UserManager userManager = new UserManager(fileHandler);
        UserInteractor userInteractor = new UserInteractor(userManager, logger, fileHandler);

        RunApplication(userInteractor, logger);
    }

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
