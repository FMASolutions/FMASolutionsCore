using System;
using FMASolutionsCore.DataServices.CryptoHelper;
using FMASolutionsCore.CLITools.CLIHelper;
namespace FMASolutionsCore.CLITools.CryptoTester
{
    public class CryptoTester
    {
        private enum EncDecOption
        {
            Encryption,
            Decryption
        }

        public CryptoTester()
        {
            firstRun = true;
            userSalt = "";
            userKey = "";
            userSourceText = "";
        }

        private string userSourceText;
        private string userKey;
        private string userSalt;
        private bool firstRun;
        private bool userWantsToRunAgain;
        private bool userWantsToKeepKeyAndSalt;
        EncDecOption currentEncDecOption;

        public void Run()
        {
            if (firstRun)
            {
                DisplayWelcomeMessage();
                firstRun = false;
                currentEncDecOption = GetEncDecOption();
                userSourceText = GetUserPlainText();
                userKey = GetUserEncryptionKey();
                userSalt = GetUserSalt();
            }
            else
            {
                currentEncDecOption = GetEncDecOption();
                userSourceText = GetUserPlainText();
                userWantsToKeepKeyAndSalt = CheckIfUserWantsToUpdateKeyOrSalt();
                if (!userWantsToKeepKeyAndSalt)
                {
                    userKey = GetUserEncryptionKey();
                    userSalt = GetUserSalt();
                }
            }
            if (currentEncDecOption == EncDecOption.Encryption)
                DisplayEncryptedDataToUser(userSourceText, CryptoService.Encrypt(userSourceText, userKey, userSalt));
            else if (currentEncDecOption == EncDecOption.Decryption)
                DisplayDecryptedDataToUser(userSourceText, CryptoService.Decrypt(userSourceText, userKey, userSalt));

            userWantsToRunAgain = CheckIfUserWantsToRunAgain();

            if (userWantsToRunAgain)
                Run();
        }

        private void DisplayWelcomeMessage() => Console.WriteLine("Welcome to Crypto Tester.");

        private EncDecOption GetEncDecOption()
        {
            Console.WriteLine("Would you like to Encrypt Or Decrypt?:");
            Console.WriteLine("1) Select \"1\" For Encryption ");
            Console.WriteLine("2) Select \"2\" For Decryption ");
            string returnString = Console.ReadLine();
            if (returnString == "1")
                return EncDecOption.Encryption;
            else if (returnString == "2")
                return EncDecOption.Decryption;
            else
                Helper.DisplayRetryOrQuit();
            return GetEncDecOption();
        }

        private bool CheckIfUserWantsToUpdateKeyOrSalt()
        {
            return Helper.GetYesNoAnswerFromUser("Would you like to keep your current salt value of: \"" + userSalt + "\" and KEY value of: \"" + userKey + "\"?");
        }
        private void DisplayEncryptedDataToUser(string userPlainText, string encryptedData)
        {
            Console.WriteLine("Encrypted data for: \"" + userPlainText + "\"");
            Console.WriteLine("looks like: \"" + encryptedData + "\"");
            Console.WriteLine("Thanks for using the service!");
        }
        private void DisplayDecryptedDataToUser(string userEncryptedData, string decryptedData)
        {
            Console.WriteLine("Decrypted data for: \"" + userEncryptedData + "\"");
            Console.WriteLine("looks like: \"" + decryptedData + "\"");
            Console.WriteLine("Thanks for using the service!");
        }
        private bool CheckIfUserWantsToRunAgain()
        {
            return Helper.GetYesNoAnswerFromUser("Would you like to run again?");
        }
        private string GetUserPlainText()
        {
            Console.WriteLine("Please enter the plain text you wish to operate on:");
            return Helper.GetUserInput();
        }
        private string GetUserEncryptionKey()
        {
            Console.WriteLine("Plese enter the KEY to use:");
            return Helper.GetUserInput();
        }
        private string GetUserSalt()
        {
            Console.WriteLine("Please enter the SALT to use:");
            return Helper.GetUserInput();
        }
    }
}