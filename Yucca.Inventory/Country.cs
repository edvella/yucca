namespace Yucca.Inventory;

public class Country
{
    public string Name { get; set; }
    public string IsoCode { get; set; }

    public override string ToString()
    {
        return IsoCode + " - " + Name;
    }
}
