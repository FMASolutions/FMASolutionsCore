using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.ViewModels
{
    public class SearchOrderViewModel
    {
        public SearchOrderViewModel()
        {

        }
        public int OrderIDUserInput  {get;set;}
        public string CustomerCodeUserInput {get;set;}
        public Dictionary<int,string> CustomersWithOrdersDictionary {get;set;}       
        public string StatusMessage {get;set;}
    }
}