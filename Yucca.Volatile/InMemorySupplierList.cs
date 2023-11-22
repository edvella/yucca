using System;
using System.Collections.Generic;
using Yucca.Inventory;

namespace Yucca.Volatile;

public class InMemorySupplierList : SupplierList
{
    protected List<Supplier> Suppliers = new();

    public override Supplier GetFirst()
    {
        return Suppliers[0];
    }

    public override void Save(Supplier supplier)
    {
        Suppliers.Add(supplier);
    }

    public override IEnumerable<Supplier> FilterByName(string text)
    {
        return Suppliers.FindAll(s => 
            s.Name.Contains(text, StringComparison.OrdinalIgnoreCase));
    }
}
