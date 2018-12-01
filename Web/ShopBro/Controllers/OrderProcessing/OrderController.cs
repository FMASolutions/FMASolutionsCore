using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class OrderController : BaseController
    {
        public IActionResult AmendItems(int id=0)
        {
            OrderModel model = GetNewModel();
            var vm =  model.GetAmendOrderItemsViewModel(id);
            return View("AmendItems",vm);
        }
        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }
        private IOrderService _orderService;
        private IOrderItemService _orderItemService;
        
        public IActionResult Index()
        {
            return View("Search",new GenericSearchViewModel());
        }
        
        [HttpGet]
        public IActionResult Search(int id=0)
        {
            
            GenericSearchViewModel vm = new GenericSearchViewModel();
            if(id > 0)
            {
                vm.ID = id;
                return ProcessSearch(vm);
            }
            else
                return View("Search",vm);
                
        }
        
        [HttpPost]
        public IActionResult ProcessSearch(GenericSearchViewModel vmInput)
        {            
            OrderModel model = GetNewModel();            
            OrderViewModel vm = model.Search(vmInput.ID);
            
            if(vm.OrderID > 0)
                return View("Display",vm);
            else
            {
                vmInput.StatusMessage = "Not found";
                return View("Search", vmInput);
            }
        }            

        [HttpGet]
        public IActionResult DisplayAll()
        {
            OrderModel model = GetNewModel();
            OrdersViewModel ordersVM = model.GetAllOrders();
            if(ordersVM != null && ordersVM.Orders.Count > 0)
            {
                return View("DisplayAll",ordersVM);
            }
            else
            {
                return View("Search",new GenericSearchViewModel());
            }
        }

       [HttpGet]
        public IActionResult EditItems(int id=0)
        {
            OrderModel model = GetNewModel();
            return View("EditItems", model.Search(id));
        }
        
        [HttpPost]
        public IActionResult ProcessEditItems(OrderViewModel vm)
        {
            OrderModel model = GetNewModel();

            OrderViewModel updatedVM = model.UpdateItems(vm);
            if(updatedVM != null)
                return View("Display",updatedVM);
            else
            {
                vm.StatusMessage = "Unable to update Order";                
                return View("EditItems",vm);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            OrderModel model = GetNewModel();            
            OrderViewModel vm = model.GetDefaultViewModel();
            return View("Create",vm);
        }

        [HttpPost]
        public IActionResult Create(OrderViewModel vm)
        {
            OrderModel model = GetNewModel();
            int orderHeaderID = model.CreateOrder(vm);
            if(orderHeaderID > 0)
                return Search(orderHeaderID);
            else
                return View("Create",vm);
            
        }
         
        private OrderModel GetNewModel()
        {
            return new OrderModel(new ModelStateConverter(this).Convert(), _orderService, _orderItemService);
        }
    }
}
