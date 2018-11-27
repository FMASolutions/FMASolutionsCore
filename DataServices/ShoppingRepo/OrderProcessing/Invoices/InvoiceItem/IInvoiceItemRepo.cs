using FMASolutionsCore.DataServices.DataRepository;
using System.Collections.Generic;

namespace FMASolutionsCore.DataServices.ShoppingRepo
{
    public interface IInvoiceItemRepo : IDataRepository<InvoiceItemEntity>
    {  
        IEnumerable<InvoiceItemEntity> GetAllItemsForInvoice(int invoiceHeaderID);

    }
}