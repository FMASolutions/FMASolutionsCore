namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    public interface IPoEMarketData
    {
        bool FirstSearchSuccessful { get; }
        bool ReverseSearchSuccessful { get; }
        string Item1Location { get; }
        string Item2Location { get; }
        double Item1Worth { get; }
        double Item1Cost { get; }
        void PerformSearch(PoECore.ePoEItemList itemReceive, PoECore.ePoEItemList itemGive);
    }
}