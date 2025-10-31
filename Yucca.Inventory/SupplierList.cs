using System.Collections.Generic;

namespace Yucca.Inventory;

public abstract class SupplierList
{    
    public string Save(Supplier supplier)
    {
        if (string.IsNullOrEmpty(supplier.Id))
            return Create(supplier);
        else
            return Update(supplier);
    }

    public abstract string Create(Supplier supplier);

    public abstract string Update(Supplier supplier);

    public abstract Supplier GetFirst();

    public abstract IEnumerable<Supplier> FilterByName(string text);

    public abstract void Remove(string id);
    
    public abstract Supplier Get(string id);
}
