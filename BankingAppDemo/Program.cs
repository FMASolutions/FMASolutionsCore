using System;
namespace BankingAppDemo
{
    class Program
    {
        static void Main(string[] args)
        {      
            Console.ReadLine();      
            Account account = new Account();
            //string balance = account.GetAccountBalance("AC123");
            string newstring = account.GitTest();
            //Console.WriteLine("Balance = " + balance);
            Console.ReadLine();
        }
    }
}
