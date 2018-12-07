using System.Collections.Generic;
using System;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class DisplayInvoiceViewModel
    {
        public DisplayInvoiceViewModel()
        {
            InvoiceItems = new List<InvoiceItemDTO>();
        }
        public List<InvoiceItemDTO> InvoiceItems {get;set;}
        public string InvoiceStatus {get;set;}
        public decimal InvoiceTotal { 
            get{
                decimal runningTotal = 0.0m;
                foreach(var item in InvoiceItems)
                    runningTotal += item.ItemTotal;
                return runningTotal;
            }
        }
        public string StatusMessage{get;set;}

    }
}