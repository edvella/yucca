using System;
using System.Threading.Tasks;
using Yucca.Inventory;
using Yucca.Migration;

namespace Yucca;

public class MigrationService(AccessDataReader accessReader, ISupplierList supplierList)
{
    private readonly AccessDataReader _accessReader = accessReader;
    private readonly ISupplierList _supplierList = supplierList;

    public async Task<MigrationResult> MigrateAsync(string path, string password)
    {
        try
        {
            Console.WriteLine("📖 Reading suppliers from Access database...");
            var suppliers = await _accessReader.ReadSuppliersAsync(path, password);
            Console.WriteLine($"   Found {suppliers.Count} suppliers to migrate.\n");

            if (suppliers.Count == 0)
            {
                return new MigrationResult { Success = true, MigratedCount = 0 };
            }

            Console.WriteLine("💾 Writing suppliers to SQL Server...");
            int successCount = 0;

            foreach (var supplier in suppliers)
            {
                try
                {
                    await _supplierList.Save(supplier);
                    successCount++;
                    Console.WriteLine($"   ✓ {supplier.Name}");
                }
                catch (Exception ex)
                {
                    CommandLine.ShowError($"   ✗ Failed to migrate {supplier.Name}: {ex.Message}");
                }
            }

            return new MigrationResult
            {
                Success = successCount == suppliers.Count,
                MigratedCount = successCount
            };
        }
        catch (Exception ex)
        {
            return new MigrationResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}