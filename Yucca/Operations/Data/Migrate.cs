using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Yucca.Operations.Data
{
    public class Migrate(MigrationService migrationService) : IYuccaOperation
    {
        private readonly MigrationService _migrationService = migrationService;

        public static string RegisterCommand() => "migrate";

        public async Task Execute(string[] parameters)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CommandLine.ShowError("❌ The 'migrate' command is only available on Windows.");
                CommandLine.ShowWarning($"   Current platform: {RuntimeInformation.OSDescription}");
                Environment.Exit(1);
                return;
            }

            try
            {
                var args = CommandLine.ParseNamedArgs(parameters, 1);
                var path = CommandLine.Get(args, "path");
                var password = CommandLine.Get(args, "password");

                Console.WriteLine("🔄 Starting Supplier Data Migration from Access to SQL Server...\n");

                var result = await _migrationService.MigrateAsync(path, password);

                if (result.Success)
                {
                    CommandLine.ShowSuccess($"\n✅ Migration completed successfully!");
                    Console.WriteLine($"   Suppliers migrated: {result.MigratedCount}");
                }
                else
                {
                    CommandLine.ShowError($"\n❌ Migration failed: {result.ErrorMessage}");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                CommandLine.ShowError($"\n❌ An unexpected error occurred: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
