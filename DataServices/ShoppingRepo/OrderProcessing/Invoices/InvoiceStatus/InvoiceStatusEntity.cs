using System;
using FMASolutionsCore.DataServices.DataRepository;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public class InvoiceStatusEntity : IBaseEntity
    {
        public InvoiceStatusEntity()
        {
        }
        public InvoiceStatusEntity(Int32 invoiceStatusID, string invoiceStatusValue)
        {
            _invoiceStatusID = invoiceStatusID;
            _invoiceStatusValue = invoiceStatusValue;
        }

        protected Int32 _invoiceStatusID;
        protected string _invoiceStatusValue;
        public Int32 ID { get { return _invoiceStatusID; } set { _invoiceStatusID = value; } }
        public Int32 InvoiceStatusID { get { return _invoiceStatusID; } set { _invoiceStatusID = value; } }
        public string InvoiceStatusValue { get { return _invoiceStatusValue; } set { _invoiceStatusValue = value; } }
        
    }
}