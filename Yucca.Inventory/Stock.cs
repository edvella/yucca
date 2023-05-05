using System.Collections.Generic;
using System.Linq;

namespace Yucca.Inventory;

public abstract class Stock
{
    public int Size()
    {
        return Items().Count();
    }

    public void AddItem(Item item, int quantity = 1)
    {
        if (Items().Any(_ => _.ItemCode == item.ItemCode))
        {

        }
        else
        {
            item.InStock = quantity;
            InsertItem(item);
        }
    }

    protected abstract void InsertItem(Item item);

    public abstract IEnumerable<Item> Items();
}
