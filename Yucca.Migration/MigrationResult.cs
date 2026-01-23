namespace Yucca.Migration;

public class MigrationResult
{
    public bool Success { get; set; }
    public int MigratedCount { get; set; }
    public string ErrorMessage { get; set; }
}