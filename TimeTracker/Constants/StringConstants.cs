﻿namespace TimeTracker.Constants;




public static class StringConstants
{
    public static readonly IList<string> Authorization = new List<string> { "Login", "Register", "Exit" };

    public static readonly string Greetings = @"
     /$$      /$$           /$$                                                     /$$                     /$$$$$$$$                  /$$       /$$  /$$$$$$                 /$$ /$$
    | $$  /$ | $$          | $$                                                    | $$                    |__  $$__/                 | $$      |__/ /$$__  $$               | $$| $$
    | $$ /$$$| $$  /$$$$$$ | $$  /$$$$$$$  /$$$$$$  /$$$$$$/$$$$   /$$$$$$        /$$$$$$    /$$$$$$          | $$  /$$$$$$   /$$$$$$$| $$   /$$ /$$| $$  \__//$$   /$$      | $$| $$
    | $$/$$ $$ $$ /$$__  $$| $$ /$$_____/ /$$__  $$| $$_  $$_  $$ /$$__  $$      |_  $$_/   /$$__  $$         | $$ |____  $$ /$$_____/| $$  /$$/| $$| $$$$   | $$  | $$      | $$| $$
    | $$$$_  $$$$| $$$$$$$$| $$| $$      | $$  \ $$| $$ \ $$ \ $$| $$$$$$$$        | $$    | $$  \ $$         | $$  /$$$$$$$|  $$$$$$ | $$$$$$/ | $$| $$_/   | $$  | $$      |__/|__/
    | $$$/ \  $$$| $$_____/| $$| $$      | $$  | $$| $$ | $$ | $$| $$_____/        | $$ /$$| $$  | $$         | $$ /$$__  $$ \____  $$| $$_  $$ | $$| $$     | $$  | $$              
    | $$/   \  $$|  $$$$$$$| $$|  $$$$$$$|  $$$$$$/| $$ | $$ | $$|  $$$$$$$        |  $$$$/|  $$$$$$/         | $$|  $$$$$$$ /$$$$$$$/| $$ \  $$| $$| $$     |  $$$$$$$       /$$ /$$
    |__/     \__/ \_______/|__/ \_______/ \______/ |__/ |__/ |__/ \_______/         \___/   \______/          |__/ \_______/|_______/ |__/  \__/|__/|__/      \____  $$      |__/|__/
                                                                                                                                                              /$$  | $$              
                                                                                                                                                             |  $$$$$$/              
                                                                                                                                                              \______/               
        ";

    public static readonly string AppName = @"
         ________                    __        __   ______            
        |        \                  |  \      |  \ /      \           
         \$$$$$$$$______    _______ | $$   __  \$$|  $$$$$$\ __    __ 
           | $$  |      \  /       \| $$  /  \|  \| $$_  \$$|  \  |  \
           | $$   \$$$$$$\|  $$$$$$$| $$_/  $$| $$| $$ \    | $$  | $$
           | $$  /      $$ \$$    \ | $$   $$ | $$| $$$$    | $$  | $$
           | $$ |  $$$$$$$ _\$$$$$$\| $$$$$$\ | $$| $$      | $$__/ $$
           | $$  \$$    $$|       $$| $$  \$$\| $$| $$       \$$    $$
            \$$   \$$$$$$$ \$$$$$$$  \$$   \$$ \$$ \$$       _\$$$$$$$
                                                            |  \__| $$
                                                             \$$    $$
                                                              \$$$$$$ 
        ";

    public static readonly string MenuOptions = "\n[B] - Back   [Enter] - Select   [↑/↓] - Navigate";

    public static readonly List<string> options = new()
    {
        "Create Project",
        "Create Task",
        "Create Subtask",
        "Start Timer on Subtask",
        "Stop Timer on Subtask",
        "Logout"
    };

    public static readonly string maxCharacterWarning = "The string size must be within 20 !";

    public static readonly string[] UserRoles = { "Regular User", "Manager" };

}