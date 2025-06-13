using System;
using System.Collections;
using System.Collections.Generic;

namespace Yucca.Inventory;

public abstract class SupplierList
{    
    public abstract string Save(Supplier supplier);

    public abstract Supplier GetFirst();

    public abstract IEnumerable<Supplier> FilterByName(string text);

    public abstract void Remove(string id);
    
    public abstract Supplier Get(string id);
}
