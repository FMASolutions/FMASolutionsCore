using System;
using Newtonsoft.Json;

namespace BankingAppDemo
{
    public class BalanceInstruction
    {
        [JsonProperty("Data")]
        public Data Data { get; set; }

        [JsonProperty("Links")]
        public Links Links { get; set; }

        [JsonProperty("Meta")]
        public Meta Meta { get; set; }
    }

    public class Data
    {
        [JsonProperty("Balance")]
        public Balance[] Balance { get; set; }
    }

    public class Balance
    {
        [JsonProperty("AccountId")]
        public string AccountId { get; set; }

        [JsonProperty("Amount")]
        public Amount Amount { get; set; }

        [JsonProperty("CreditDebitIndicator")]
        public string CreditDebitIndicator { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("DateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("CreditLine")]
        public CreditLine[] CreditLine { get; set; }
    }

    public class Amount
    {
        [JsonProperty("Amount")]
        public string AmountAmount { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }
    }

    public class CreditLine
    {
        [JsonProperty("Included")]
        public bool Included { get; set; }

        [JsonProperty("Amount")]
        public Amount Amount { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }

    public class Links
    {
        [JsonProperty("Self")]
        public string Self { get; set; }

        [JsonProperty("Prev")]
        public string Prev { get; set; }

        [JsonProperty("Next")]
        public string Next { get; set; }
    }

    public class Meta
    {
        [JsonProperty("TotalPages")]
        public long TotalPages { get; set; }
    }

}
