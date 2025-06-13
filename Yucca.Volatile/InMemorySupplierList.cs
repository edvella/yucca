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

    public override string Save(Supplier supplier)
    {
        supplier.Id = Guid.NewGuid().ToString();
        Suppliers.Add(supplier);

        return supplier.Id;
    }

    public override IEnumerable<Supplier> FilterByName(string text)
    {
        return Suppliers.FindAll(s => 
            s.Name.Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    public override void Remove(string id)
    {
        Suppliers.RemoveAll(_ => _.Id == id);
    }

    public override Supplier Get(string id)
    {
        return Suppliers.Find(s => s.Id == id);
    }
}
