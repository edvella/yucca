using Yucca.Inventory;

namespace Yucca.Web.Services;

internal sealed class SupplierService(HttpClient httpClient, ILogger<SupplierService> logger)
{
    public async Task<List<Supplier>> GetSuppliers()
    {
        List<Supplier>? suppliers = null;

        var response = await httpClient.GetAsync("/api/supplier");

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

    public async Task<bool> AddSupplier(Supplier supplier)
    {
        var response = await httpClient.PostAsJsonAsync("/api/supplier", supplier);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        logger.LogWarning("Failed to add supplier. StatusCode: {StatusCode}", response.StatusCode);
        return false;
    }
}
