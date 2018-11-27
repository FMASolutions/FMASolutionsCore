using System;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.ShoppingService
{
    public interface IInvoiceService : IDisposable
    {
        Invoice GenerateInvoiceForOrder(int orderHeaderID);
        List<Invoice> GetInvoicesForOrder(int orderHeaderID);
        Invoice GetInvoiceByInvoiceID(int invoiceHeaderID);
        Dictionary<int, string> GetInvoiceStatusDic();
        Order GetOrder(int orderHeaderID);
    }
}