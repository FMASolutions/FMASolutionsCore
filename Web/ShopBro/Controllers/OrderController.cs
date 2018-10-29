using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        private IOrderService _service;
        public IActionResult Search(int id=0)
        {
            GenericSearchViewModel vm = new GenericSearchViewModel();
            vm.ID = id;
            return ProcessSearch(vm);
        }

        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {
            OrderModel model = GetNewModel();
            
            
            OrderViewModel vm = new OrderViewModel();
            List<OrderItemViewModel> currentItems = new List<OrderItemViewModel>();
            List<ItemViewModel> availableItems = new List<ItemViewModel>();
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
            item1.OrderItemRowID = 786;
            item2.OrderItemRowID = 321;
            currentItems.Add(item1);
            currentItems.Add(item2);


            availableCustomers.Add(1,"FMA Solutions LTD!");
            availableCustomers.Add(2,"Some Other Company");

            ItemViewModel availItem1 = new ItemViewModel();
            availItem1.ItemAvailableQty = 50;
            availItem1.ItemCode = "Code1";
            availItem1.ItemDescription = "First Item Description";
            availItem1.ItemID = 26;
            availItem1.ItemName = "First Item Name";
            availItem1.ItemUnitPrice = 2.99m;
            availItem1.ItemUnitPriceWithMaxDiscount = 1.99m;
            availItem1.SubGroupID = 1;


            availableItems.Add(availItem1);
            
            
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

        private OrderModel GetNewModel()
        {
            return new OrderModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}