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

                public InvoiceViewModel GenerateInvoiceForOrder(int orderHeaderID)
        {
            InvoiceViewModel returnInvoice = null;
            Invoice inv = _service.GenerateInvoiceForOrder(orderHeaderID);
            if(inv != null)
                returnInvoice = ConvertToInvoiceViewModel(inv.Header, inv.Items);
            return returnInvoice;            
        }
        public List<InvoiceViewModel> GetInvoicesByOrder(int orderHeaderID)
        {
            List<InvoiceViewModel> returnInvoices = new List<InvoiceViewModel>();
            var invoices = _service.GetInvoicesForOrder(orderHeaderID);
            
            if(invoices != null && invoices.Count > 0)
                foreach(var invoice in invoices)
                    returnInvoices.Add(ConvertToInvoiceViewModel(invoice.Header, invoice.Items));
            
            if(returnInvoices.Count > 0)
                return returnInvoices;
            else
                return null;
        }
        public InvoiceViewModel GetInvoiceByInvoiceID(int invoiceID)
        {
            InvoiceViewModel returnVM = new InvoiceViewModel();
            var searchInvoice = _service.GetInvoiceByInvoiceID(invoiceID);
            if(searchInvoice != null)
            {
                returnVM = ConvertToInvoiceViewModel(searchInvoice.Header, searchInvoice.Items);
                return returnVM;
            }
            else
                return null;
        }


        private InvoiceViewModel ConvertToInvoiceViewModel(InvoiceHeader header, IEnumerable<InvoiceItem> items)
        {
            InvoiceViewModel returnViewModel = new InvoiceViewModel();
            Dictionary<int,string> invoiceStatusDic = _service.GetInvoiceStatusDic();
            Order orderForInvoice = _service.GetOrder(header.OrderHeaderID);

            returnViewModel.InvoiceDate = header.InvoiceDate;
            returnViewModel.InvoiceHeaderID = header.InvoiceHeaderID;
            returnViewModel.InvoiceStatus = invoiceStatusDic[header.InvoiceStatusID];
            returnViewModel.OrderHeaderID = header.OrderHeaderID;
            
            foreach(var item in items)
            {
                InvoiceItemViewModel currentItem = new InvoiceItemViewModel();
                OrderItem currentOrderItem = orderForInvoice.OrderItems.Find(e => e.OrderItemID == item.OrderItemID);
                currentItem.InvoiceHeaderID = item.InvoiceHeaderID;
                currentItem.InvoiceItemID = item.InvoiceItemID;
                currentItem.InvoiceItemQty = item.InvoiceItemQty;
                currentItem.InvoiceItemStatus = invoiceStatusDic[item.InvoiceItemStatusID];
                currentItem.OrderItemID = item.OrderItemID;
                currentItem.ItemDescription = currentOrderItem.OrderItemDescription;
                currentItem.ItemPrice = currentOrderItem.OrderItemUnitPrice;

                returnViewModel.Items.Add(currentItem);                
            }
            return returnViewModel;
        }


    }
    
}