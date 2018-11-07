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
                return View("DisplayForUpdate",vm);
            else
                return View("Search");
        }    

        public IActionResult Update(OrderViewModel model)
        {
            OrderViewModel received = model;
            
            return View("Search");
        }   

        private OrderModel GetNewModel()
        {
            return new OrderModel(new ModelStateConverter(this).Convert(), _service);
        }
    }
}