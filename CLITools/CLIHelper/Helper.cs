using System;

namespace FMASolutionsCore.CLITools.CLIHelper
{
    public class Helper
    {
        public static bool GetYesNoAnswerFromUser(string initialMessage)
        {
            Console.WriteLine(initialMessage);
            Console.WriteLine("1) Type \"1\" for yes");
            Console.WriteLine("2) Type \"2\" for no");
            string userInput = GetUserInput().ToLower();
            if (userInput == "1" || userInput == "yes" || userInput == "y")
                return true;
            else if (userInput == "2" || userInput == "no" || userInput == "n")
                return false;
            else
                DisplayRetryOrQuit();
            return GetYesNoAnswerFromUser(initialMessage);
        }

        public static void DisplayRetryOrQuit()
        {
            Console.WriteLine("Invalid option detected. type \"quit\" (or just q) to exit or \"continue\" (or just c) to try again");
            string userInput = GetUserInput().ToLower();
            if (userInput == "quit" || userInput == "q")
            {
                Environment.Exit(0);
            }
            else if (userInput == "continue" || userInput == "c")
            {
                //Do nothing, let the function drop out and execute fall back to where it last was.
            }
            else //User input invalid option again.... Maybe they are a robot... Lets kill them with the power of recursion!
            {
                DisplayRetryOrQuit();
            }
        }

        public static string GetUserInput()
        {
            string returnString = Console.ReadLine();
            return returnString;
        }
    }
}
