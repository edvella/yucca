using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Yucca.Inventory;

namespace Yucca.Web.Components.Pages
{
    public partial class Suppliers
    {
        private List<Supplier>? _suppliers;
        private string _searchText = string.Empty;
        private System.Timers.Timer? _debounceTimer;
        private const int DebounceDelayMilliseconds = 500;
        private string formLabelStyle = "block mb-2 text-sm font-medium text-gray-900";
        private string formElementStyle = "bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5";
        private string buttonStyle = "inline-flex items-center px-5 py-2.5 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg text-sm transition-colors duration-200 mb-2 mr-2";

        private string _selectedCountry = string.Empty;
        private string selectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                if (string.IsNullOrEmpty(_selectedCountry))
                {
                    _supplier!.Country = null;
                }
                else
                {
                    _supplier!.Country = new Country
                    {
                        Name = _countries.Single(_ => _.IsoCode.Equals(_selectedCountry)).Name,
                        IsoCode = _selectedCountry
                    };
                }
            }
        }

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
            selectedCountry = _supplier.Country.IsoCode;
            _isNewSupplier = false;

            StateHasChanged();
        }

        private void CreateNewSupplier()
        {
            _supplier = new();
            _isNewSupplier = true;
        }

        private async Task SaveSupplier()
        {
            if (await SupplierService.SaveSupplier(_supplier!))
            {
                await RefreshSupplierList();
                await JSRuntime.InvokeVoidAsync("alert", "Supplier saved successfully!");
                ClearForm();
            }
            StateHasChanged();
        }

        private void ClearForm()
        {
            _supplier = new();
            _isNewSupplier = true;
            selectedCountry = string.Empty;
        }

        private async Task ExportCsv()
        {
            try
            {
                var list = _suppliers ?? new List<Supplier>();
                var csvLines = new List<string>
                {
                    "Id,Name,AddressLine1,AddressLine2,City,State,PostCode,CountryIso,CountryName,ContactPhone,Email,Website,TaxNumber"
                };

                foreach (var s in list)
                {
                    string countryIso = s.Country?.IsoCode ?? string.Empty;
                    string countryName = s.Country?.Name ?? string.Empty;

                    string[] fields = [
                        EscapeCsv(s.Id),
                        EscapeCsv(s.Name),
                        EscapeCsv(s.AddressLine1),
                        EscapeCsv(s.AddressLine2),
                        EscapeCsv(s.City),
                        EscapeCsv(s.State),
                        EscapeCsv(s.PostCode),
                        EscapeCsv(countryIso),
                        EscapeCsv(countryName),
                        EscapeCsv(s.ContactPhone),
                        EscapeCsv(s.Email),
                        EscapeCsv(s.Website),
                        EscapeCsv(s.TaxNumber)
                    ];

                    csvLines.Add(string.Join(',', fields));
                }

                var csv = string.Join("\r\n", csvLines);
                var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
                var base64 = Convert.ToBase64String(bytes);
                var filename = $"Suppliers_{DateTime.Now:yyyyMMddHHmmss}.csv";

                await JSRuntime.InvokeVoidAsync("blazorDownloadFile", filename, "text/csv;charset=utf-8;", base64);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting CSV: {ex.Message}");
                await JSRuntime.InvokeVoidAsync("alert", "Failed to export CSV");
            }
        }

        private static string EscapeCsv(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var needsQuotes = input.Contains(',') || input.Contains('\"') || input.Contains('\n') || input.Contains('\r');
            var escaped = input.Replace("\"", "\"\"");
            return needsQuotes ? $"\"{escaped}\"" : escaped;
        }
    }
}
