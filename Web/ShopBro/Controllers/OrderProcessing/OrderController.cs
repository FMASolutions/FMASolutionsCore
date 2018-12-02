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
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        private IOrderService _orderService;

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
            OrderModel model = GetOrderModel();            
            DisplayOrderViewModel vm = model.Search(vmInput.ID);
            
            if(vm != null)
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
            OrderModel model = GetOrderModel();
            DisplayAllOrdersViewModel ordersVM = model.GetAllOrders();
            if(ordersVM != null && ordersVM.Orders.Count > 0)
                return View("DisplayAll",ordersVM);
            else
                return View("Search",new GenericSearchViewModel());
        }

        [HttpGet]
        public IActionResult AmendItems(int id=0)
        {
            OrderModel model = GetOrderModel();
            var vm =  model.GetAmendOrderItemsViewModel(id);
            return View("AmendItems",vm);
        }
        
        [HttpPost]
        public IActionResult ProcessAmendItems(AmendOrderItemsViewModel vm)
        {
            OrderModel model = GetOrderModel();

            DisplayOrderViewModel updatedVM = model.UpdateItems(vm);
            if(updatedVM != null)
                return View("Display",updatedVM);
            else
            {
                vm.StatusMessage = "Unable to update Order";                
                return View("AmendItems",vm);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            OrderModel model = GetOrderModel();            
            CreateOrderViewModel emptyVM = model.GetEmptyCreateModel();
            return View("Create",emptyVM);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderViewModel vm)
        {
            OrderModel model = GetOrderModel();
            int orderHeaderID = model.CreateOrder(vm);
            if(orderHeaderID > 0)
                return Search(orderHeaderID);
            else
                return View("Create",vm);
            
        }
         
        private OrderModel GetOrderModel()
        {
            return new OrderModel(new ModelStateConverter(this).Convert(), _orderService);
        }
    }
}
