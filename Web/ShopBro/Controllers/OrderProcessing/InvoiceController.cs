using FMASolutionsCore.BusinessServices.ControllerTemplate;
using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.Web.ShopBro.Models;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

namespace FMASolutionsCore.Web.ShopBro.Controllers
{
    public class InvoiceController : BaseController
    {
        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        private IInvoiceService _service;

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
        public IActionResult ViewInvoice(int id=0)
        {
            InvoiceModel model = GetNewModel();
            DisplayInvoiceViewModel searchResult = model.GetInvoiceByInvoiceID(id);
            if(searchResult != null)
                return View("DisplayInvoice",searchResult);
            else
                return null;
        }

        private InvoiceModel GetNewModel()
        {
            return new InvoiceModel(new ModelStateConverter(this).Convert(), _service);
        }
    
        
    }
}