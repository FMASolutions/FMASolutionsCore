using System.Collections.Generic;
using System;
namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            Items = new List<InvoiceItemViewModel>();
            InvoiceDate = DateTime.Now;
        }

        public List<InvoiceItemViewModel> Items;
        public int InvoiceHeaderID {get;set;}
        public int OrderHeaderID {get;set;}
        public string InvoiceStatus {get;set;}
        public DateTime InvoiceDate{get;set;}
        public string StatusMessage {get;set;}
        public decimal InvoiceTotal {get 
        { 
            decimal tot = 0.0m; 
            if(Items != null && Items.Count > 0)
            foreach(var item in Items)
                tot += item.ItemTotal;
            return tot;
        }}

    }

    public class InvoiceItemViewModel
    {
        public InvoiceItemViewModel()
        {

        }

        public int InvoiceItemID {get;set;}
        public int InvoiceHeaderID {get;set;}
        public int OrderItemID {get;set;}
        public string InvoiceItemStatus{get;set;}
        public int InvoiceItemQty{get;set;}
        public string ItemDescription {get;set;}
        public decimal ItemPrice {get;set;}
        public decimal ItemTotal {get { return ItemPrice * InvoiceItemQty;}}
    }
}