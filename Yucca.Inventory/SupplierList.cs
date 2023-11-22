using System.Collections;
using System.Collections.Generic;

namespace Yucca.Inventory;

public abstract class SupplierList
{    
    public abstract void Save(Supplier supplier);

    public abstract Supplier GetFirst();

    public abstract IEnumerable<Supplier> FilterByName(string text);
}
