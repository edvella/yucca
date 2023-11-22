using FluentAssertions;
using System.Linq;
using Xunit;
using Yucca.Inventory;
using Yucca.Volatile;

namespace YuccaTests;

public class InventoryTest
{
    private readonly Stock inventory;

    private readonly Item socks = new Item
    {
        ItemCode = "S1",
        Description = "Socks"
    };

    public InventoryTest()
    {
        inventory = new ListInventory();
    }

    [Fact]
    public void CanInitialiseInventory()
    {
        inventory.Size().Should().Be(0);
    }

    [Fact]
    public void CanAddItemToInventory()
    {
        inventory.AddItem(new Item());
        inventory.Size().Should().Be(1);
    }

    [Fact]
    public void AddingNewSingleItemWillShowStockOf1()
    {
        inventory.AddItem(socks);
        inventory.Items().First().InStock.Should().Be(1);
    }

    [Fact]
    public void AddingNewItemsWithQuantity3WillShowStockOf3()
    {
        inventory.AddItem(socks, 3);
        inventory.Items().First().InStock.Should().Be(3);
    }
}
