using System.Net.Http;
using Newtonsoft.Json;

namespace BankingAppDemo
{
    public class Account
    {
        public string GetAccountBalance(string AccountID)
        {
            string baseURL = "http://127.0.0.1:1880/Accounts/" + AccountID + "/Balances?";
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
    }
}