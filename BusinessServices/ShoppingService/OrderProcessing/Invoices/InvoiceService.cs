using System;
using System.Collections.Generic;
using FMASolutionsCore.DataServices.ShoppingRepo;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;

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

        public Invoice GenerateInvoiceForOrder(int orderHeaderID)
        {
            int invoiceHeaderID = _uow.InvoiceHeaderRepo.GenerateInvoiceForOrder(orderHeaderID);
            InvoiceHeaderEntity invoiceHeader = _uow.InvoiceHeaderRepo.GetByID(invoiceHeaderID);
            IEnumerable<InvoiceItemEntity> invoiceItems = _uow.InvoiceItemRepo.GetAllItemsForInvoice(invoiceHeaderID);
            Invoice returnInvoice = ConvertInvoiceEntityToModel(invoiceHeader,invoiceItems);
            if(returnInvoice != null && returnInvoice.Items.Count > 0)
            {
                _uow.SaveChanges();
                return returnInvoice;
            }
            else
                return null;
        }
        public List<Invoice> GetInvoicesForOrder(int orderHeaderID)
        {
            List<Invoice> returnInvoiceList = new List<Invoice>();
            IEnumerable<int> invoiceHeaderIDList = _uow.InvoiceHeaderRepo.GetInvoicesForOrder(orderHeaderID);
            foreach(int ID in invoiceHeaderIDList)
                returnInvoiceList.Add(ConvertInvoiceEntityToModel(_uow.InvoiceHeaderRepo.GetByID(ID), _uow.InvoiceItemRepo.GetAllItemsForInvoice(ID)));
            if(returnInvoiceList.Count > 0)
                return returnInvoiceList;
            else
                return null;
        }
        public Invoice GetInvoiceByInvoiceID(int invoiceHeaderID)
        {
            var header = _uow.InvoiceHeaderRepo.GetByID(invoiceHeaderID);
            var items = _uow.InvoiceItemRepo.GetAllItemsForInvoice(invoiceHeaderID);
            Invoice returnInvoice = ConvertInvoiceEntityToModel(header,items);
            if(returnInvoice != null && returnInvoice.Items.Count > 0)
            {
                return returnInvoice;
            }
            else
                return null;
        }
        public Dictionary<int, string> GetInvoiceStatusDic()
        {
            Dictionary<int,string> returnDic = new Dictionary<int, string>();

            foreach(var item in _uow.InvoiceStatusRepo.GetAll())
                returnDic.Add(item.InvoiceStatusID, item.InvoiceStatusValue);
            
            return returnDic;
        }
        public Order GetOrder(int orderHeaderID)
        {
            IOrderService orderService = new OrderService(_uow);
            return orderService.GetByID(orderHeaderID);
        }
        

        private Invoice ConvertInvoiceEntityToModel(InvoiceHeaderEntity headerEntity, IEnumerable<InvoiceItemEntity> itemsEntity)
        {
            Invoice returnInvoice = new Invoice();
            returnInvoice.Header = new InvoiceHeader(headerEntity.InvoiceHeaderID, headerEntity.OrderHeaderID, headerEntity.InvoiceStatusID, headerEntity.InvoiceDate);
            foreach(var invoiceItem in itemsEntity)
                returnInvoice.Items.Add(new InvoiceItem(invoiceItem.InvoiceItemID,invoiceItem.InvoiceHeaderID,invoiceItem.OrderItemID,invoiceItem.InvoiceItemStatusID,invoiceItem.InvoiceItemQty));
            return returnInvoice;
        } 


    }
}