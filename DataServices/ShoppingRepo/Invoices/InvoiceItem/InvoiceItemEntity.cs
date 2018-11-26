using FMASolutionsCore.DataServices.DataRepository;
using System;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class InvoiceItemEntity : IBaseEntity
    {
        public InvoiceItemEntity()
        {
        }
        public InvoiceItemEntity(Int32 invoiceItemID, Int32 invoiceHeaderID, Int32 orderItemID, Int32 invoiceITemStatusID, Int32 invoiceItemQty)
        {
            _invoiceItemID = invoiceItemID;
            _invoiceHeaderID = invoiceHeaderID;
            _orderItemID = orderItemID;
            _invoiceItemStatusID = invoiceITemStatusID;
            _invoiceItemQty = invoiceItemQty;
        }

        private Int32 _invoiceItemID;
        private Int32 _invoiceHeaderID;
        private Int32 _orderItemID;
        private Int32 _invoiceItemStatusID;
        private Int32 _invoiceItemQty;

        public Int32 ID {get; set;}
        public Int32 InvoiceItemID {get {return _invoiceItemID;} set{_invoiceItemID = value;}}
        public Int32 InvoiceHeaderID {get {return _invoiceHeaderID;} set{_invoiceHeaderID = value;}}
        public Int32 OrderItemID {get {return _orderItemID;} set{_orderItemID = value;}}
        public Int32 InvoiceItemStatusID {get { return _invoiceItemStatusID;} set { _invoiceItemStatusID = value;}}
        public Int32 InvoiceItemQty {get {return _invoiceItemQty;} set{_invoiceItemQty = value;}}

    }
}