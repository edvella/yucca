using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using Yucca.Inventory;
using Yucca.Volatile;

namespace YuccaTests;

public class SupplierTests
{
    readonly SupplierList suppliers;

    public SupplierTests()
    {
        suppliers = new InMemorySupplierList();
    }

    [Fact]
    public void CanInitialiseSupplierList()
    {
    }

    [Fact]
    public void CanStoreSupplierDetails()
    {
        AddSupplier();

        var result = suppliers.GetFirst();
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
    public void CanUpdateSupplierDetails()
    {
        var id = AddSupplier();

        var supplier = suppliers.Get(id);
        supplier.Name = "Supplier 1 Updated";
        supplier.AddressLine1 = "New Address Line 1";
        supplier.ContactPhone = "New Contact Phone";

        suppliers.Save(supplier);

        var result = suppliers.Get(id);

        result.Should().NotBeNull();
        result.Name.Should().Be("Supplier 1 Updated");
        result.AddressLine1.Should().Be("New Address Line 1");
        result.ContactPhone.Should().Be("New Contact Phone");
    }

    [Fact]
    public void NewSupplierWillHaveAnId()
    {
        var result = AddSupplier();

        Guid.Parse(result).Should().NotBeEmpty();
    }

    private string AddSupplier()
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

        return suppliers.Save(supplier);
    }

    [Fact]
    public void CanGetSupplierById()
    {
        var id = AddSupplier();

        var result = suppliers.Get(id);

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
    public void CanFilterSupplierListByName()
    {
        suppliers.Save(new Supplier { Name = "Space Toys Co. Ltd." });
        suppliers.Save(new Supplier { Name = "Omega Widgets" });
        suppliers.Save(new Supplier { Name = "Moon Industries" });
        suppliers.Save(new Supplier { Name = "Moon Patrol Tools" });

        var result = suppliers.FilterByName("Moon");
        
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.Name == "Moon Industries");
        result.Should().Contain(s => s.Name == "Moon Patrol Tools");
    }

    [Fact]
    public void EmptySearchStringResultsInFullSupplierList()
    {
        suppliers.Save(new Supplier { Name = "Space Toys Co. Ltd." });
        suppliers.Save(new Supplier { Name = "Omega Widgets" });
        suppliers.Save(new Supplier { Name = "Moon Industries" });
        suppliers.Save(new Supplier { Name = "Moon Patrol Tools" });

        var result = suppliers.FilterByName("");

        result.Should().HaveCount(4);
        result.Should().Contain(s => s.Name == "Space Toys Co. Ltd.");
        result.Should().Contain(s => s.Name == "Omega Widgets");
        result.Should().Contain(s => s.Name == "Moon Industries");
        result.Should().Contain(s => s.Name == "Moon Patrol Tools");
    }

    [Fact]
    public void CanRemoveSupplier()
    {
        var id = AddSupplier();

        suppliers.Remove(id);

        var result = suppliers.FilterByName("");

        result.Should().HaveCount(0);
    }

    [Fact]
    public void CanRemoveRequiredSupplier()
    {
        var id2 = suppliers.Save(new Supplier { Name = "Omega Widgets" });
        var id = AddSupplier();

        suppliers.Remove(id);

        var result = suppliers.FilterByName("");

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(id2);
        result.First().Name.Should().Be("Omega Widgets");
    }
}