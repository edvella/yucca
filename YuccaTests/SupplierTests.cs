using FluentAssertions;
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
        var supplier = new Supplier
        {
            Name = "Supplier 1",
            AddressLine1 = "Address Line 1",
            AddressLine2 = "Address Line 2",
            City = "City",
            State = "State",
            PostCode = "Post Code",
            Country = "Country",
            ContactPhone = "Contact Phone",
            Email = "Email",
            Website = "Website",
            TaxNumber = "Tax Number"
        };

        suppliers.Save(supplier);
        
        var result = suppliers.GetFirst();
        result.Name.Should().Be("Supplier 1");
        result.AddressLine1.Should().Be("Address Line 1");
        result.AddressLine2.Should().Be("Address Line 2");
        result.City.Should().Be("City");
        result.State.Should().Be("State");
        result.PostCode.Should().Be("Post Code");
        result.Country.Should().Be("Country");
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
}