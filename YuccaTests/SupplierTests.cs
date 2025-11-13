using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Yucca.Inventory;
using Yucca.Volatile;

namespace YuccaTests;

public class SupplierTests
{
    readonly ISupplierList suppliers;

    public SupplierTests()
    {
        suppliers = new InMemorySupplierList();
    }

    [Fact]
    public void CanInitialiseSupplierList()
    {
    }

    [Fact]
    public async Task CanStoreSupplierDetails()
    {
        await AddSupplier();

        var result = (await suppliers.FilterByName(""))!.First();
        result.Name.Should().Be("Supplier 1");
        result.AddressLine1.Should().Be("Address Line 1");
        result.AddressLine2.Should().Be("Address Line 2");
        result.City.Should().Be("City");
        result.State.Should().Be("State");
        result.PostCode.Should().Be("Post Code");
        result.Country.Name.Should().Be("United States");
        result.Country.IsoCode.Should().Be("US");
        result.ContactPhone.Should().Be("Contact Phone");
        result.Email.Should().Be("Email");
        result.Website.Should().Be("Website");
        result.TaxNumber.Should().Be("Tax Number");
    }

    [Fact]
    public async Task CanUpdateSupplierDetails()
    {
        var id = await AddSupplier();

        var supplier = await suppliers.Get(id);
        supplier.Name = "Supplier 1 Updated";
        supplier.AddressLine1 = "New Address Line 1";
        supplier.ContactPhone = "New Contact Phone";

        await suppliers.Save(supplier);

        var result = await suppliers.Get(id);

        result.Should().NotBeNull();
        result.Name.Should().Be("Supplier 1 Updated");
        result.AddressLine1.Should().Be("New Address Line 1");
        result.ContactPhone.Should().Be("New Contact Phone");
    }

    [Fact]
    public async Task NewSupplierWillHaveAnId()
    {
        var result = await AddSupplier();

        Guid.Parse(result).Should().NotBeEmpty();
    }

    private async Task<string> AddSupplier()
    {
        var supplier = new Supplier
        {
            Name = "Supplier 1",
            AddressLine1 = "Address Line 1",
            AddressLine2 = "Address Line 2",
            City = "City",
            State = "State",
            PostCode = "Post Code",
            Country = new Country { Name = "United States", IsoCode = "US" },
            ContactPhone = "Contact Phone",
            Email = "Email",
            Website = "Website",
            TaxNumber = "Tax Number"
        };

        return await suppliers.Save(supplier);
    }

    [Fact]
    public async Task CanGetSupplierById()
    {
        var id = await AddSupplier();

        var result = await suppliers.Get(id);

        result.Should().NotBeNull();
        result.Name.Should().Be("Supplier 1");
        result.AddressLine1.Should().Be("Address Line 1");
        result.AddressLine2.Should().Be("Address Line 2");
        result.City.Should().Be("City");
        result.State.Should().Be("State");
        result.PostCode.Should().Be("Post Code");
        result.Country.Name.Should().Be("United States");
        result.Country.IsoCode.Should().Be("US");
        result.ContactPhone.Should().Be("Contact Phone");
        result.Email.Should().Be("Email");
        result.Website.Should().Be("Website");
        result.TaxNumber.Should().Be("Tax Number");
    }

    [Fact]
    public async Task CanFilterSupplierListByName()
    {
        await suppliers.Save(new Supplier { Name = "Space Toys Co. Ltd." });
        await suppliers.Save(new Supplier { Name = "Omega Widgets" });
        await suppliers.Save(new Supplier { Name = "Moon Industries" });
        await suppliers.Save(new Supplier { Name = "Moon Patrol Tools" });

        var result = (await suppliers.FilterByName("Moon")).ToList();
        
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.Name == "Moon Industries");
        result.Should().Contain(s => s.Name == "Moon Patrol Tools");
    }

    [Fact]
    public async Task EmptySearchStringResultsInFullSupplierList()
    {
        await suppliers.Save(new Supplier { Name = "Space Toys Co. Ltd." });
        await suppliers.Save(new Supplier { Name = "Omega Widgets" });
        await suppliers.Save(new Supplier { Name = "Moon Industries" });
        await suppliers.Save(new Supplier { Name = "Moon Patrol Tools" });

        var result = (await suppliers.FilterByName("")).ToList();

        result.Should().HaveCount(4);
        result.Should().Contain(s => s.Name == "Space Toys Co. Ltd.");
        result.Should().Contain(s => s.Name == "Omega Widgets");
        result.Should().Contain(s => s.Name == "Moon Industries");
        result.Should().Contain(s => s.Name == "Moon Patrol Tools");
    }

    [Fact]
    public async Task CanRemoveSupplier()
    {
        var id = await AddSupplier();

        await suppliers.Remove(id);

        var result = (await suppliers.FilterByName("")).ToList();

        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task CanRemoveRequiredSupplier()
    {
        var id2 = await suppliers.Save(new Supplier { Name = "Omega Widgets" });
        var id = await AddSupplier();

        await suppliers.Remove(id);

        var result = (await suppliers.FilterByName("")).ToList();

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(id2);
        result.First().Name.Should().Be("Omega Widgets");
    }
}