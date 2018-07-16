using FMASolutionsCore.DataServices.WebHelper;
using FMASolutionsCore.BusinessServices.BusinessCore;
using System;

namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    internal class PoEWebMarket : IPoEMarketData
    {
        private const string _searchPrefix = "&#10799; <div class=\"currencyimg cur20-";
        private const string _searchSuffix = "\"></div> ";
        private const string _searchSuffixCost = "&larr; ";
        private const string _searchSuffixWorth = "&rarr; ";
        private bool _firstSearchSuccessful;
        private bool _reverseSearchSuccessful;
        private string _item1Location;
        private string _item2Location;
        private double _item1Worth;
        private double _item1Cost;
        private string _currentSourceCode;

        public bool FirstSearchSuccessful{get{return this._firstSearchSuccessful;}}

        public bool ReverseSearchSuccessful{get{return this._reverseSearchSuccessful;}}

        public string Item1Location{get{return this._item1Location;}}

        public string Item2Location{get{return this._item2Location;}}

        public double Item1Worth{get{return this._item1Worth;}}

        public double Item1Cost{get{return this._item1Cost;}}

        public void PerformSearch(PoECore.ePoEItemList itemReceive, PoECore.ePoEItemList itemGive)
        {
            this.SetupURLs((int)itemReceive, (int)itemGive);
            this._currentSourceCode = WebCrawler.HTMLResultFromGETRequest(this._item1Location);
            if (this.CheckTradeIsAvailable((int)itemReceive))
            {
                this._firstSearchSuccessful = true;
                this._item1Worth = this.GetTradeValue((int)itemReceive, (int)itemGive, false);
                this._currentSourceCode = WebCrawler.HTMLResultFromGETRequest(this._item2Location);
                if (this.CheckTradeIsAvailable((int)itemGive))
                {
                    this._reverseSearchSuccessful = true;
                    this._item1Cost = this.GetTradeValue((int)itemGive, (int)itemReceive, true);
                }
                else
                    this._reverseSearchSuccessful = false;
            }
            else
                this._firstSearchSuccessful = false;
        }

        private void SetupURLs(int itemNumber1, int itemNumber2)
        {
            this._item1Location = "http://currency.poe.trade/search?league=Bestiary&online=x&want=" + itemNumber1.ToString() + "&have=" + (object)itemNumber2;
            this._item2Location = "http://currency.poe.trade/search?league=Bestiary&online=x&want=" + itemNumber2.ToString() + "&have=" + (object)itemNumber1;
        }

        private string GenerateSearchString(int ItemNumber, bool IsForCost = true)
        {
            if (IsForCost)
                return "&#10799; <div class=\"currencyimg cur20-" + ItemNumber.ToString() + "\"></div> &larr; ";
            return "&#10799; <div class=\"currencyimg cur20-" + ItemNumber.ToString() + "\"></div> &rarr; ";
        }

        private bool CheckTradeIsAvailable(int itemNumber)
        {
            if (this._currentSourceCode.Contains(this.GenerateSearchString(itemNumber, true)))
                return true;
            DateTime now = DateTime.Now;
            PoEProfitHunter.LoggerService.WriteToCustomLog("NoTradesLog", (now.ToShortDateString() + " @ " + now.ToLongTimeString() + ":     No Trades for: " + itemNumber.ToString() + " and: " + itemNumber.ToString()));
            return false;
        }

        private double GetTradeValue(int itemNumber1, int itemNumber2, bool ValueAsCost = true)
        {
            string str = this._currentSourceCode.Substring(!ValueAsCost ? this._currentSourceCode.IndexOf("&rarr; ", this._currentSourceCode.IndexOf(this.GenerateSearchString(itemNumber2, false))) + "&rarr; ".Length : this._currentSourceCode.IndexOf("&larr; ", this._currentSourceCode.IndexOf(this.GenerateSearchString(itemNumber1, true))) + "&larr; ".Length);
            int num = str.IndexOf(" ");
            return double.Parse(str.Substring(0, num + 1));
        }
    }
}