using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IInvoiceHeaderRepo : IDataRepository<InvoiceHeaderEntity>
    {  
        int GetLatestInvoiceHeaderID();
        int GenerateInvoiceForOrder(int orderHeaderID);
        IEnumerable<int> GetInvoicesForOrder(int orderHeaderID);
    }
}