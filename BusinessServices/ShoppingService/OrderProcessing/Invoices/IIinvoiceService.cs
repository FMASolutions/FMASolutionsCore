using System;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IInvoiceService : IDisposable
    {
        int GenerateInvoiceForOrder(int orderHeaderID);
        IEnumerable<int> GetInvoicesForOrder(int orderHeaderID);
        Dictionary<int, string> GetInvoiceStatusDic();
        IEnumerable<InvoiceItemDTO> GetInvoiceItemsByInvoiceID(int invoiceID);
    }
}