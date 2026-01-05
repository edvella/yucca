using System.Threading.Tasks;

namespace Yucca.Operations.Supplier;

public class List(SupplierOps supplierOps) : IYuccaOperation
{
    private readonly SupplierOps _supplierOps = supplierOps;

    public static string RegisterCommand() => "supplier list";

    public async Task Execute(string[] parameters)
    {
        await _supplierOps.ListSuppliers();
    }
}
