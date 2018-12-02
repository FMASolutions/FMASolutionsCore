using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using Microsoft.AspNetCore.Authorization;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class InvoiceController : BaseController
    {
        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        private IInvoiceService _service;

        [Authorize(Policy = "Admin")]
        public IActionResult GenInvoice(int id=0) //id = orderID
        {
            InvoiceModel model = GetNewModel();
            int invoiceID = model.GenerateInvoiceForOrder(id);
            DisplayInvoiceViewModel vm = model.GetInvoiceByInvoiceID(invoiceID);
            if(vm != null)
                return View("DisplayInvoice",vm);
            else
                return null;
        }
        [Authorize(Policy = "Admin")]
        public IActionResult ViewInvoice(int id=0)
        {
            InvoiceModel model = GetNewModel();
            DisplayInvoiceViewModel searchResult = model.GetInvoiceByInvoiceID(id);
            if(searchResult != null)
                return View("DisplayInvoice",searchResult);
            else
                return null;
        }

        [Authorize(Policy = "Admin")]
        private InvoiceModel GetNewModel()
        {
            return new InvoiceModel(new ModelStateConverter(this).Convert(), _service);
        }
    
        
    }
}