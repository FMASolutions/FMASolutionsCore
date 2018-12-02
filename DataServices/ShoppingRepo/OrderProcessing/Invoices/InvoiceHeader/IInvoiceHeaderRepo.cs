using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingDTOFactory;
namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IInvoiceHeaderRepo : IDataRepository<InvoiceHeaderEntity>
    {  
        int GetLatestInvoiceHeaderID();
        int GenerateInvoiceForOrder(int orderHeaderID);
        IEnumerable<int> GetInvoicesForOrder(int orderHeaderID);
        IEnumerable<InvoiceItemDTO> GetInvoiceByInvoiceID(int invoiceID);
    }
}