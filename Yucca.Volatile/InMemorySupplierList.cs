using System;
using System.Collections.Generic;
using Yucca.Inventory;

namespace Yucca.Volatile;

public class InMemorySupplierList : SupplierList
{
    protected List<Supplier> Suppliers = [];

    public override Supplier GetFirst()
    {
        return Suppliers[0];
    }

    public override string Create(Supplier supplier)
    {
        supplier.Id = Guid.NewGuid().ToString();
        Suppliers.Add(supplier);

        return supplier.Id;
    }

    public override string Update(Supplier supplier)
    {
        Suppliers.RemoveAll(s => s.Id == supplier.Id);
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
