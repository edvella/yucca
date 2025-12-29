using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text;
using Yucca.Inventory;
using Yucca.Output;

namespace Yucca.Web.Components.Pages
{
    public partial class Suppliers
    {
        private List<Supplier>? _suppliers;
        private string _searchText = string.Empty;
        private System.Timers.Timer? _debounceTimer;
        private const int DebounceDelayMilliseconds = 500;

        public void Dispose()
        {
            _debounceTimer?.Dispose();
        }

        [SupplyParameterFromForm]
        private Supplier? _supplier { get; set; }

        private List<Country> _countries;

        private bool _isNewSupplier = true;

        protected override async Task OnInitializedAsync()
        {
            using (StreamReader r = new StreamReader("data/countries.json"))
            {
                string json = r.ReadToEnd();
                _countries = JsonConvert.DeserializeObject<List<Country>>(json);
            }

            _supplier ??= new();
            _debounceTimer = new (DebounceDelayMilliseconds);
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += async (sender, args) => await InvokeAsync(FilterSuppliers);
            await RefreshSupplierList();
        }

        private async Task RefreshSupplierList()
        {
            await FilterSuppliers();
        }

        private async Task FilterSuppliers()
        {
            try
            {
                _suppliers = await SupplierService.GetSuppliers(_searchText);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FilterSuppliers: {ex.Message}");
            }
        }

        private void StartSearchTimer()
        {
            _debounceTimer?.Stop();
            _debounceTimer?.Start();
        }

        private async Task HandleKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                _debounceTimer?.Stop();
                await FilterSuppliers();
            }
        }

        private async Task SelectSupplier(string id)
        {
            _supplier = await SupplierService.GetSupplierById(id);
            _isNewSupplier = false;

            StateHasChanged();
        }

        private async Task SaveSupplier()
        {
            if (await SupplierService.SaveSupplier(_supplier!))
            {
                await RefreshSupplierList();
                NotificationService.ShowSuccess("Supplier added successfully!");
                ClearForm();
            }
            StateHasChanged();
        }

        private void ClearForm()
        {
            _supplier = new();
            _isNewSupplier = true;
        }

        private async Task DeleteSupplier(string id)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this supplier?");
            if (!confirmed) return;

            if (await SupplierService.DeleteSupplier(id))
            {
                await RefreshSupplierList();
                NotificationService.ShowSuccess("Supplier deleted");
            }
            else
            {
                NotificationService.ShowError("Failed to delete supplier");
            }
        }

        private async Task SupplierDetails(string id)
        {
            NotificationService.ShowWarning("Feature not yet available!");
        }

        private async Task ClearSearch()
        {
            _searchText = string.Empty;
            _debounceTimer?.Stop();
            await RefreshSupplierList();
        }

        private async Task ExportCsv()
        {
            try
            {
                var list = _suppliers ?? new List<Supplier>();
                var csv = CsvExporter.GenerateSupplierCsv(list);
                var bytes = Encoding.UTF8.GetBytes(csv);
                var base64 = Convert.ToBase64String(bytes);
                var filename = $"Suppliers_{DateTime.Now:yyyyMMddHHmmss}.csv";

                await JSRuntime.InvokeVoidAsync("blazorDownloadFile", filename, "text/csv;charset=utf-8;", base64);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting CSV: {ex.Message}");
                NotificationService.ShowError("Failed to export CSV");
            }
        }

        private async Task PrintSuppliers()
        {
            try
            {
                var list = _suppliers ?? new List<Supplier>();

                var sb = new StringBuilder();
                sb.AppendLine($"<h1 style=\"font-family: Arial, Helvetica, sans-serif;\">Supplier List</h1>");
                sb.AppendLine($"<div style=\"font-family: Arial, Helvetica, sans-serif; margin-bottom: 1rem;\">Printed: {DateTime.Now:f}</div>");

                sb.AppendLine("<table style=\"width:100%; border-collapse:collapse; font-family: Arial, Helvetica, sans-serif;\">");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th style=\"border:1px solid #ddd; padding:8px; text-align:left;\">Name</th>");
                sb.AppendLine("<th style=\"border:1px solid #ddd; padding:8px; text-align:left;\">Address</th>");
                sb.AppendLine("<th style=\"border:1px solid #ddd; padding:8px; text-align:left;\">Phone</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");

                foreach (var s in list)
                {
                    string Escape(string? input) => string.IsNullOrEmpty(input) ? string.Empty : System.Net.WebUtility.HtmlEncode(input);

                    var addressParts = new[] { s.AddressLine1, s.AddressLine2, s.City, s.Country?.IsoCode }
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Select(p => Escape(p));

                    var address = string.Join(", ", addressParts);

                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td style=\"border:1px solid #ddd; padding:8px; vertical-align:top;\">{Escape(s.Name)}</td>");
                    sb.AppendLine($"<td style=\"border:1px solid #ddd; padding:8px; vertical-align:top;\">{address}</td>");
                    sb.AppendLine($"<td style=\"border:1px solid #ddd; padding:8px; vertical-align:top;\">{Escape(s.ContactPhone)}</td>");
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                await JSRuntime.InvokeVoidAsync("printHtml", sb.ToString());
            }
            catch (Exception ex)
            {
                NotificationService.ShowError($"Printing failed: {ex.Message}");
            }
        }
    }
}
