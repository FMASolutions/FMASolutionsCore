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
            
            OrderViewModel vm = model.Search(vmInput.ID);
            
            if(vmInput.ID > 0)
                return View("Display",vm);
            else
                return View("Search", vmInput);
        }            

        public IActionResult EditItems(int id=0)
        {
            OrderModel model = GetNewModel();
            return View("EditItems", model.Search(id));
        }
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

        public IActionResult DeliverItems(int id=0)
        {
            if(id>0)
            {
                OrderModel model = GetNewModel();
                int deliveryNoteID = model.DeliverItems(id);
                return View("DeliveryNote",model.GetDeliveryNote(deliveryNoteID));
            }
            else
                return Search();
        }
        public IActionResult GenInvoice(int id=0)
        {
            return null;
        }
        public IActionResult ViewInvoice(int id=0)
        {
            return null;
        }
        private OrderModel GetNewModel()
        {
            return new OrderModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}