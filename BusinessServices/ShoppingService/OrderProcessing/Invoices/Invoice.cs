using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.DataServices.ShoppingRepo;
using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class Invoice : IModel
    {
        public Invoice()
        {
            _items = new List<InvoiceItem>();
        }

        public List<InvoiceItem> Items {get{ return _items; } set{ _items = value; }}
        public InvoiceHeader Header {get{ return _header;} set{ _header = value;} }
        public ICustomModelState ModelState {get{return _modelState;} set{_modelState = value;}}
        private ICustomModelState _modelState;
        private List<InvoiceItem> _items;
        private InvoiceHeader _header;
    }

    public class InvoiceItem : InvoiceItemEntity
    {
        public InvoiceItem(int invoiceItemID, int invoiceHeaderID, int orderItemID, int invoiceItemStatusID, int invoiceItemQty)
            :base(invoiceItemID, invoiceHeaderID, orderItemID, invoiceItemStatusID, invoiceItemQty)
        {

        }
    }
    public class InvoiceHeader : InvoiceHeaderEntity
    {
        public InvoiceHeader(int invoiceHeaderID, int orderHeaderID, int invoiceStatusID, DateTime invoiceDate)
            :base(invoiceHeaderID, orderHeaderID,invoiceStatusID, invoiceDate)
        {

        }
    }
}
