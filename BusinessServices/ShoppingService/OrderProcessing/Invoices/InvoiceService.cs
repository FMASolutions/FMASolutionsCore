using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public class InvoiceService : IInvoiceService
    {
        public InvoiceService(string connectionString, SQLAppConfigTypes.SQLAppConfigTypes dbType)
        {
            _uow = new UnitOfWork(connectionString, dbType);
        }
        internal InvoiceService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public void Dispose()
        {
            if(!_disposing)
            {
                _disposing = true;
                _uow.Dispose();
            }
        }
        
        private bool _disposing = false;
        private IUnitOfWork _uow;

        public int GenerateInvoiceForOrder(int orderHeaderID)
        {
            int invoiceHeaderID = _uow.InvoiceHeaderRepo.GenerateInvoiceForOrder(orderHeaderID);
            if(invoiceHeaderID > 0)
                _uow.SaveChanges();
            return invoiceHeaderID;            
        }
        public IEnumerable<int> GetInvoicesForOrder(int orderHeaderID)
        {
            IEnumerable<int> returnList = _uow.InvoiceHeaderRepo.GetInvoicesForOrder(orderHeaderID);
            return returnList;
        }
        public Dictionary<int, string> GetInvoiceStatusDic()
        {
            Dictionary<int,string> returnDic = new Dictionary<int, string>();

            foreach(var item in _uow.InvoiceStatusRepo.GetAll())
                returnDic.Add(item.InvoiceStatusID, item.InvoiceStatusValue);
            
            return returnDic;
        }

        public IEnumerable<InvoiceItemDTO> GetInvoiceItemsByInvoiceID(int invoiceID)
        {
            return _uow.InvoiceHeaderRepo.GetInvoiceByInvoiceID(invoiceID);
        }
    }
}