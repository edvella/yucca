using System;
using System.ComponentModel.DataAnnotations;

namespace Yucca.Inventory;

public class Supplier
{
    public string Id { get; set; }

    private string _name = string.Empty;

    [Required]
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new SupplierWithoutName();
            _name = value;
        }
    }

    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public Country Country { get; set; }
    public string PostCode { get; set; }
    public string ContactPhone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string TaxNumber { get; set; }
}

public class SupplierWithoutName : Exception
{
    public SupplierWithoutName() : base("Name cannot be null or empty.")
    {
    }
}