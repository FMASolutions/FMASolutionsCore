using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class OrderController : BaseController
    {
        public IActionResult Search(int id=0)
        {
            GenericSearchViewModel vm = new GenericSearchViewModel();
            vm.ID = id;
            return ProcessSearch(vm);
        }

        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            OrderViewModel vm = new OrderViewModel();
            List<OrderItemViewModel> currentItems = new List<OrderItemViewModel>();
            Dictionary<int,string> availableItems = new Dictionary<int, string>();
            Dictionary<int,string> availableCustomers = new Dictionary<int, string>();

            OrderItemViewModel item1 = new OrderItemViewModel();
            OrderItemViewModel item2 = new OrderItemViewModel();
            item1.ItemID = 31;
            item2.ItemID = 83;
            item1.Qty = 5;
            item2.Qty = 3;
            item1.ItemDescription = "Jet Wash";
            item2.ItemDescription = "Shower Head";
            item1.UnitPrice = 3.50m;
            item2.UnitPrice = 4.75m;
            currentItems.Add(item1);
            currentItems.Add(item2);


            availableCustomers.Add(1,"FMA Solutions LTD!");
            availableCustomers.Add(2,"Some Other Company");

            availableItems.Add(63,"Jet Washer Pro");
            availableItems.Add(81,"Blagger");
            availableItems.Add(44,"PS4");
            availableItems.Add(736,"Xbox");
            availableItems.Add(189,"WiiU");

            
            vm.CustomerID = 1;
            vm.ExistingItems = currentItems;
            vm.AvailableItems = availableItems;
            vm.AvailableCustomers = availableCustomers;
            vm.OrderID = vmInput.ID;
            vm.OrderStatus = "Estimate";
            
            
            if(vmInput.ID > 0)
                return View("Display",vm);
            else
                return View("Search");
        }       
    }
}