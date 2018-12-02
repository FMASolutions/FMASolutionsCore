using System;
using System.Collections.Generic;
using FMASolutionsCore.Web.ShopBro.ViewModels;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingService;

namespace FMASolutionsCore.Web.ShopBro.Models
{
    public class InvoiceModel : IModel, IDisposable
    {
        public InvoiceModel(ICustomModelState modelState, IInvoiceService invoiceService)
        {
            _modelState = modelState;
            _service = invoiceService;
        }
        public void Dispose()
        {           
            _service.Dispose();
        }

        private ICustomModelState _modelState;
        private IInvoiceService _service;
        public ICustomModelState ModelState { get { return _modelState; } }

        public int GenerateInvoiceForOrder(int orderHeaderID)
        {
            return _service.GenerateInvoiceForOrder(orderHeaderID);            
        }
        public List<int> GetInvoicesByOrder(int orderHeaderID)
        {
            var searchResult = _service.GetInvoicesForOrder(orderHeaderID);
            List<int> returnInvoices = null;
            if(searchResult != null)
            {
                returnInvoices = new List<int>();
                foreach(var invoice in searchResult)
                    returnInvoices.Add(invoice);
            }
            return returnInvoices;
        }
        public DisplayInvoiceViewModel GetInvoiceByInvoiceID(int invoiceID)
        {
            DisplayInvoiceViewModel returnVM = null;
            var searchInvoiceItems = _service.GetInvoiceItemsByInvoiceID(invoiceID);
            if(searchInvoiceItems != null)
            {
                returnVM =  new DisplayInvoiceViewModel();
                foreach(var item in searchInvoiceItems)
                    returnVM.InvoiceItems.Add(item);
            }
            return returnVM;
        }
    }    
}