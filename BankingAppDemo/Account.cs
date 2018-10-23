using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;

namespace BankingAppDemo
{
    public class Account
    {
        private static string serverAddress = "http://127.0.0.1:1880";
        public string GetAccountBalance(string AccountID)
        {
            string baseURL = serverAddress + "/Accounts/" + AccountID + "/Balances";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = client.GetAsync(baseURL).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = content.ReadAsStringAsync().Result;
                        if (data != null)
                        {
                            BalanceInstruction ins = JsonConvert.DeserializeObject<BalanceInstruction>(data);
                            return ins.Data.Balance[0].Amount.AmountAmount;
                        }
                        else
                            return null;
                    }
                }
            }
        }

        public string GetTransactions(string AccountID)
        {
            string baseURL = serverAddress + "/Accounts/" + AccountID + "/Transactions";
            using(HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = client.GetAsync(baseURL).Result)
                {
                    return "";
                }
            }


        }

        public string GitTest()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            HttpResponseMessage res = client.GetAsync("https://api.github.com/orgs/dotnet/repos").Result;            
            HttpContent content = res.Content;
            string data = content.ReadAsStringAsync().Result;
            return data;
        }

        
    }
}