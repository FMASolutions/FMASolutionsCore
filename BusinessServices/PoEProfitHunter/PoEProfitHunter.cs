using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.BusinessCore;

namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    public class PoEProfitHunter
    {
        private PoECore.ePoEItemList _receivingItemProcessing;
        private PoECore.ePoEItemList _givingItemProcessing;
        private string _receivingItemOriginal;
        private string _givingItemOriginal;
        private double _profitValue;
        private string _profitLossReport;
        private double _receivingItemWorth;
        private double _receivingItemCost;
        private List<ProfitHunterResult> _searchResults;
        internal static AppLoggerExtension.IAppLoggerExtension LoggerService = new AppLoggerExtension.AppLoggerExtension();

        public List<ProfitHunterResult> ExecuteBatchSearch(PoECore.ePoEItemList StartItemReceive, PoECore.ePoEItemList EndItemGive)
        {
            this._searchResults = new List<ProfitHunterResult>();
            this._receivingItemOriginal = StartItemReceive.ToString();
            this._givingItemOriginal = EndItemGive.ToString();
            this.CycleItemsAndPopulateResults(StartItemReceive, EndItemGive);
            return this._searchResults;
        }

        private void CycleItemsAndPopulateResults(PoECore.ePoEItemList StartItemReceive, PoECore.ePoEItemList EndItemGive)
        {
            for (int index1 = (int)StartItemReceive; (PoECore.ePoEItemList)index1 <= EndItemGive; ++index1)
            {
                PoECore.ePoEItemList receivingItem = (PoECore.ePoEItemList)index1;
                for (int index2 = (int)EndItemGive; index2 > index1; --index2)
                {
                    PoECore.ePoEItemList givingItem = (PoECore.ePoEItemList)index2;
                    if (this.ExecuteSingleSearch(receivingItem, givingItem))
                        this._searchResults.Add(new ProfitHunterResult(this._receivingItemOriginal, this._givingItemOriginal, receivingItem.ToString(), givingItem.ToString(), this._profitValue.ToString()));
                }
            }
        }

        private bool ExecuteSingleSearch(PoECore.ePoEItemList receivingItem, PoECore.ePoEItemList givingItem)
        {
            this.ResetBaseValues();
            this._receivingItemProcessing = receivingItem;
            this._givingItemProcessing = givingItem;
            try
            {
                return this.CheckItemCanBeFlippedForProfit();
            }
            catch (Exception ex)
            {
                LoggerService.WriteToErrorLog(DateTime.Now.ToShortDateString() + " @ " + DateTime.Now.ToLongTimeString() + ": Error while trying to compare: " + this._receivingItemProcessing.ToString() + " With: " + ex.Message, this.ToString());
                return false;
            }
        }

        private bool CheckItemCanBeFlippedForProfit()
        {
            IPoEMarketData marketDataProduct = new PoEFactoryWorker().CreateMarketDataProduct("Web");
            marketDataProduct.PerformSearch(this._receivingItemProcessing, this._givingItemProcessing);
            if (marketDataProduct.FirstSearchSuccessful)
            {
                if (marketDataProduct.ReverseSearchSuccessful)
                {
                    this._receivingItemWorth = marketDataProduct.Item1Worth;
                    this._receivingItemCost = marketDataProduct.Item1Cost;
                    this._receivingItemOriginal = marketDataProduct.Item1Location;
                    this._givingItemOriginal = marketDataProduct.Item2Location;
                    if (this._receivingItemWorth > this._receivingItemCost)
                    {
                        this.ReportAndCalculateProfitOrLossValue(true);
                        return true;
                    }
                    this.ReportAndCalculateProfitOrLossValue(false);
                    return false;
                }
                this.ReportNoTradeAvailable(true);
                return false;
            }
            this.ReportNoTradeAvailable(false);
            return false;
        }

        private void ResetBaseValues()
        {
            this._profitValue = 0.0;
            this._receivingItemWorth = 0.0;
            this._receivingItemCost = 0.0;
        }

        private void GenerateReportLogForProfitAndLoss(string Message, FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType reportType)
        {
            switch (reportType)
            {
                case FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType.NoTradeAvailable:
                    LoggerService.WriteToCustomLog("NoTradesLog", Message);
                    break;
                case FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType.Profit:
                    LoggerService.WriteToCustomLog("ProfitLog", Message);
                    break;
                case FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType.Loss:
                    LoggerService.WriteToCustomLog("LossLog", Message);
                    break;
            }
        }

        private void ReportAndCalculateProfitOrLossValue(bool IsProfit = true)
        {
            DateTime now = DateTime.Now;
            if (IsProfit)
            {
                this._profitValue = this._receivingItemWorth - this._receivingItemCost;
                this._profitLossReport = now.ToShortDateString() + " @ " + now.ToLongTimeString();
                this._profitLossReport = this._profitLossReport + ": PROFIT FOUND: " + this._receivingItemProcessing.ToString() + " and: " + this._givingItemProcessing.ToString() + "Total Profit = ";
                this._profitLossReport = this._profitLossReport + this._profitValue.ToString() + " " + (object)this._profitValue;
                this.GenerateReportLogForProfitAndLoss(this._profitLossReport, FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType.Profit);
            }
            else
            {
                this._profitValue = this._receivingItemCost - this._receivingItemWorth;
                this._profitLossReport = now.ToShortDateString() + " @ " + now.ToLongTimeString();
                this._profitLossReport = this._profitLossReport + ": LOSS FOUND: " + this._receivingItemProcessing.ToString() + " and: " + this._givingItemProcessing.ToString() + "Total Loss = ";
                this._profitLossReport = this._profitLossReport + this._profitValue.ToString() + " " + this._receivingItemProcessing.ToString();
                this.GenerateReportLogForProfitAndLoss(this._profitLossReport, FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType.Loss);
            }
        }

        private void ReportNoTradeAvailable(bool FirstTradeCompleted = true)
        {
            this._profitLossReport = !FirstTradeCompleted ? "Unable to find INITIAL(REVERSE NOT ATTEMPTED) trade for: " + this._receivingItemProcessing.ToString() + " TO: " + this._givingItemProcessing.ToString() : "Unable to find REVERSE(INITIAL SEARCH COMPLETED) trade for: " + this._receivingItemProcessing.ToString() + " TO: " + this._givingItemProcessing.ToString();
            this.GenerateReportLogForProfitAndLoss(this._profitLossReport, FMASolutionsCore.BusinessServices.PoEProfitHunter.PoEProfitHunter.TradeReportType.NoTradeAvailable);
        }

        private enum TradeReportType
        {
            NoTradeAvailable,
            Profit,
            Loss,
        }
    }
}