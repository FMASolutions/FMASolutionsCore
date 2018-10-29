namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel()
        {

        }
        public decimal UnitPrice{get;set;}
        public decimal Qty{get;set;}
        public int ItemID{get;set;}
        public string ItemDescription {get;set;}
        public string StatusMessage {get; set;}
        public int OrderItemRowID {get;set;}

    }
}