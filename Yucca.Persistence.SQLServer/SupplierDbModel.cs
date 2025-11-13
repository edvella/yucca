namespace Yucca.Persistence.SQLServer;

internal class SupplierDbModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string CountryIsoCode { get; set; }
    public string PostCode { get; set; }
    public string ContactPhone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string TaxNumber { get; set; }
}
