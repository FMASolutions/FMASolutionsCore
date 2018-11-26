using FMASolutionsCore.DataServices.DataRepository;
using System;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class InvoiceHeaderEntity : IBaseEntity
    {
        public InvoiceHeaderEntity()
        {
            _invoiceDate = DateTime.Now;
        }
        public InvoiceHeaderEntity(Int32 invoiceHeaderID, Int32 orderHeaderID, Int32 invoiceStatusID, DateTime invoiceDate)
        {
            _invoiceHeaderID = invoiceHeaderID;
            _orderHeaderID = orderHeaderID;
            _invoiceStatusID = invoiceStatusID;
            _invoiceDate = invoiceDate;
        }
        private Int32 _invoiceHeaderID;
        private Int32 _orderHeaderID;
        private Int32 _invoiceStatusID;
        private DateTime _invoiceDate;
        public Int32 ID {get {return _invoiceHeaderID;} set{_invoiceHeaderID = value;}}
        public Int32 InvoiceHeaderID{get {return _invoiceHeaderID;} set{_invoiceHeaderID = value;}}
        public Int32 OrderHeaderID{get {return _orderHeaderID;} set{_orderHeaderID = value;}}
        public Int32 InvoiceStatusID{get {return _invoiceStatusID;} set{_invoiceStatusID = value;}}
        public DateTime InvoiceDate{get {return _invoiceDate;} set{_invoiceDate = value;}}
    }
}