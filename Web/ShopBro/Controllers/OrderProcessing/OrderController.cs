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
            
            OrderModel model = GetOrderModel();
            SearchOrderViewModel vm = model.GetEmptySearchViewodel();
            if(id > 0)
            {
                vm.OrderIDUserInput = id;
                return ProcessSearch(vm);
            }
            else
                return View("Search",vm);
                
        }
        
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult SearchByCustomer(string customerCode="")
        {
            OrderModel model = GetOrderModel();
            SearchOrderViewModel vm = model.GetEmptySearchViewodel();
            
            if(!string.IsNullOrEmpty(customerCode))
            {
                vm.CustomerCodeUserInput = customerCode;
                return ProcessSearch(vm);
            }
            else
                return View("Search",vm);
        }
        
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult ProcessSearch(SearchOrderViewModel vmInput)
        {            
            OrderModel model = GetOrderModel();
            if(vmInput.OrderIDUserInput > 0)
            {
                DisplayOrderViewModel vm = model.SearchByID(vmInput.OrderIDUserInput);
                if(vm != null && vm.OrderHeader.OrderID > 0)
                    return View("Display",vm);
                else
                    return View("Search",vm);
                
            }
            else if(!string.IsNullOrEmpty(vmInput.CustomerCodeUserInput))
            {
                DisplayOrdersViewModel vmOrders = model.SearchByCustomer(GetCustomerCodePart(vmInput.CustomerCodeUserInput));
                if(vmOrders != null && vmOrders.Orders.Count > 0 )
                    return View("DisplayMultiple", vmOrders);
                else
                    return View("Search", vmInput);
            }
            vmInput.StatusMessage = "No User Input Detected";
            return View("Search", vmInput);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult DisplayAll()
        {
            OrderModel model = GetOrderModel();
            DisplayOrdersViewModel ordersVM = model.GetAllOrders();
            if(ordersVM != null && ordersVM.Orders.Count > 0)
                return View("DisplayMultiple",ordersVM);
            else
                return View("Search",new GenericSearchViewModel());
        }

        [Authorize(Policy = "Admin")]
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
        private string GetCustomerCodePart(string inputString)
        {
            string returnString = "";
            if(inputString.Contains("-"))
                returnString = inputString.Substring(0,inputString.LastIndexOf("-")-1);
            return returnString;
            
        }
    }
}
