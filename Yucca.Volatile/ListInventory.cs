using System.Collections.Generic;
using Yucca.Inventory;

namespace Yucca.Volatile
{
    public class ListInventory : Stock
    {
        protected List<Item> Content { get; set; }

        public ListInventory()
        {
            Content = new List<Item>();
        }

        protected override void InsertItem(Item item)
        {
            Content.Add(item);
        }

        public override IEnumerable<Item> Items()
        {
            return Content;
        }
    }
}
