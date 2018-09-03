using System;
namespace BankingAppDemo
{
    class Program
    {
        static void Main(string[] args)
        {            
            Account account = new Account();
            string balance = account.GetAccountBalance("AC123");
            Console.WriteLine("Balance = " + balance);
            Console.ReadLine();
        }
    }
}
