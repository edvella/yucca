using System;
using System.Collections.Generic;

namespace Yucca.Inventory;

public abstract class SupplierList
{    
    public abstract string Save(Supplier supplier);

    public abstract Supplier Get(string id);

    public abstract IEnumerable<Supplier> FilterByName(string text);

    public abstract void Remove(string id);
}
