namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    internal abstract class PoEFactory
    {
        internal abstract IPoEMarketData CreateMarketDataProduct(string type);
    }
}