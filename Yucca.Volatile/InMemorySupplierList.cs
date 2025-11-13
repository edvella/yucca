using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yucca.Inventory;

namespace Yucca.Volatile;

public class InMemorySupplierList : ISupplierList
{
    protected List<Supplier> Suppliers = [];

    public Task<string> Create(Supplier supplier)
    {
        supplier.Id = Guid.NewGuid().ToString();
        Suppliers.Add(supplier);

        return Task.FromResult(supplier.Id);
    }

    public Task<string> Update(Supplier supplier)
    {
        Suppliers.RemoveAll(s => s.Id == supplier.Id);
        Suppliers.Add(supplier);

        return Task.FromResult(supplier.Id);
    }

    public Task<IEnumerable<Supplier>> FilterByName(string text)
    {
        var results = Suppliers.FindAll(s =>
            s.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).AsEnumerable();
        return Task.FromResult(results);
    }

    public Task Remove(string id)
    {
        Suppliers.RemoveAll(_ => _.Id == id);
        return Task.CompletedTask;
    }

    public Task<Supplier> Get(string id)
    {
        var supplier = Suppliers.Find(s => s.Id == id);
        return Task.FromResult(supplier);
    }
}
