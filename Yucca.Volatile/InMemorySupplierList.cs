using System;
using System.Collections.Generic;
using System.Linq;
using Yucca.Inventory;

namespace Yucca.Volatile;

public class InMemorySupplierList : SupplierList
{
    protected static List<Supplier> Suppliers = [];

    public override string Save(Supplier supplier)
    {
        supplier.Id = Guid.NewGuid().ToString();
        Suppliers.Add(supplier);

        return supplier.Id;
    }

    public override Supplier Get(string id)
    {
        return Suppliers.FirstOrDefault(_ => _.Id == id);
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
}
