namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    public class ProfitHunterResult
    {
        public ProfitHunterResult(string originalURL, string reversedURL, string itemReceive, string itemGive, string profitValue)
        {
            this.OriginalURL = originalURL;
            this.ReversedURL = reversedURL;
            this.ItemReceive = itemReceive;
            this.ItemGive = itemGive;
            this.ProfitValue = profitValue;
            this.UpdateProfitString();
        }

        public string OriginalURL { get; set; }

        public string ReversedURL { get; set; }

        public string ItemReceive { get; set; }

        public string ItemGive { get; set; }

        public string ProfitLineMessage { get; set; }

        public string ProfitValue { get; set; }

        private void UpdateProfitString()
        {
            this.ProfitLineMessage = "Profit for - " + this.ItemGive + " To: " + this.ItemReceive + " Profit of - " + this.ProfitValue + " " + this.ItemReceive;
        }
    }
}
