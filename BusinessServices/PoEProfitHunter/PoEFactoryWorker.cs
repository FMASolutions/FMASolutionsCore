using System;

namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    internal class PoEFactoryWorker : PoEFactory
    {
        internal override IPoEMarketData CreateMarketDataProduct(string type)
        {
            if (type == "Web")
                return (IPoEMarketData)new PoEWebMarket();
            throw new ArgumentOutOfRangeException();
        }
    }
}
