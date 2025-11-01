using Yucca.Inventory;

namespace Yucca.Web.Services;

internal sealed class SupplierService(HttpClient httpClient, ILogger<SupplierService> logger)
{
    public async Task<List<Supplier>> GetSuppliers(string nameFilter = "")
    {
        List<Supplier>? suppliers = null;

        var requestUri = string.IsNullOrWhiteSpace(nameFilter)
            ? "/api/supplier"
            : $"/api/supplier?name={Uri.EscapeDataString(nameFilter)}";

        var response = await httpClient.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            suppliers = await response.Content.ReadFromJsonAsync<List<Supplier>>();
        }

        return suppliers ?? [];
    }

    public async Task<Supplier?> GetSupplierById(string id)
    {
        var response = await httpClient.GetAsync($"/api/supplier/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Supplier>();
        }

        logger.LogWarning("Failed to fetch supplier with ID {Id}. StatusCode: {StatusCode}", id, response.StatusCode);
        return null;
    }

    public async Task<bool> SaveSupplier(Supplier supplier)
    {
        var response = await httpClient.PostAsJsonAsync("/api/supplier", supplier);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            logger.LogWarning("Failed to add supplier. StatusCode: {StatusCode}", response.StatusCode);
            return false;
        }
    }

    public async Task<bool> DeleteSupplier(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return false;

        var response = await httpClient.DeleteAsync($"/api/supplier/{id}");

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        logger.LogWarning("Failed to delete supplier with ID {Id}. StatusCode: {StatusCode}", id, response.StatusCode);
        return false;
    }
}
