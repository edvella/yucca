using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yucca.Inventory;

public interface ISupplierList
{    
    async Task<string> Save(Supplier supplier)
    {
        if (string.IsNullOrEmpty(supplier.Id))
            return await Create(supplier);
        else
            return await Update(supplier);
    }

    Task<string> Create(Supplier supplier);

    Task<string> Update(Supplier supplier);

    Task<IEnumerable<Supplier>> FilterByName(string text);

    Task Remove(string id);
    
    Task<Supplier> Get(string id);
}
