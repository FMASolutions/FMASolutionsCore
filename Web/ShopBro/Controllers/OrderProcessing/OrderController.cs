using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;
using Microsoft.AspNetCore.Authorization;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class OrderController : BaseController
    {
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        private IOrderService _orderService;

        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            OrderModel model = GetOrderModel();
            SearchOrderViewModel vm = model.GetEmptySearchViewodel();
            return View("Search",vm);
        }

        [Authorize(Policy = "Admin")]
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
        
        [Authorize(Policy = "Admin")]
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

        [Authorize(Policy = "Admin")]
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
        
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult ProcessAmendItems(AmendOrderItemsPreviewViewModel viewModel)
        {
            OrderModel model = GetOrderModel();

            DisplayOrderViewModel updatedVM = model.UpdateItems(viewModel);
            if(updatedVM != null)
                return View("Display",updatedVM);
            else     
                return AmendItems(viewModel.HeaderDetail.OrderID);            
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            OrderModel model = GetOrderModel();            
            CreateOrderViewModel emptyVM = model.GetEmptyCreateModel();
            return View("Create",emptyVM);
        }

        [Authorize(Policy = "Admin")]
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
