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
        
        public IActionResult Index()
        {
            return View("Search",new GenericSearchViewModel());
        }
        
        [HttpGet]
        public IActionResult Search(int id=0)
        {
            GenericSearchViewModel vm = new GenericSearchViewModel();
            vm.ID = id;
            return ProcessSearch(vm);
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
        public IActionResult DeliverItems(int id=0)
        {
            if(id>0)
            {
                OrderModel model = GetNewModel();
                DeliveryNoteViewModel vmReturn = model.DeliverItems(id);
                if(vmReturn != null)
                    return View("DisplayDeliveryNote",vmReturn);
                else
                    return Search();
            }
            else
                return Search();
        }
        public IActionResult ViewDeliveryNote(int id=0) //id = OrderHeaderID
        {            
            OrderModel model = GetNewModel();
            List<DeliveryNoteViewModel> deliveryNotes = model.GetDeliveryNoteByOrder(id);
            if(deliveryNotes != null)
            {
                if(deliveryNotes.Count == 1)                
                    return View("DisplayDeliveryNote", deliveryNotes[0]);
                else
                {
                    //IMPLEMENT MULTI SELECTOR FOR DELIVERY NOTE SELECTION.
                    return null;
                }
            }
            else
                return Search(id);
        }
        public IActionResult GenInvoice(int id=0) //id = OrderHeaderID
        {
            OrderModel model = GetNewModel();
            InvoiceViewModel vm = model.GenerateInvoiceForOrder(id);
            if(vm != null)
                return View("DisplayInvoice",vm);
            else
                return Search(id);
        }
        public IActionResult ViewInvoice(int id=0)
        {
            OrderModel model = GetNewModel();
            List<InvoiceViewModel> invoices = model.GetInvoicesByOrder(id);
            if(invoices != null && invoices.Count == 1)
                return View("DisplayInvoice", invoices[0]);
            else
            {
                return null;
            }
        }
        
        private OrderModel GetNewModel()
        {
            return new OrderModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}
