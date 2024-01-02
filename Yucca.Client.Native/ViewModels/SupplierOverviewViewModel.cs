using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Yucca.Volatile;

namespace Yucca.Client.Native.ViewModels;

internal class SupplierOverviewViewModel : IQueryAttributable
{
    private readonly InMemorySupplierList suppliers;
    public ObservableCollection<SupplierDetailsViewModel> Suppliers { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectItemCommand { get; }

    public SupplierOverviewViewModel()
    {
        suppliers = new InMemorySupplierList();
        Suppliers = new ObservableCollection<SupplierDetailsViewModel>(suppliers.FilterByName("").Select(n => new SupplierDetailsViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewSupplierAsync);
        SelectItemCommand = new AsyncRelayCommand<SupplierDetailsViewModel>(SelectSupplierAsync);
    }

    private async Task NewSupplierAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.SupplierDetailsPage));
    }

    private async Task SelectSupplierAsync(SupplierDetailsViewModel supplier)
    {
        if (supplier != null)
            await Shell.Current.GoToAsync($"{nameof(Views.SupplierDetailsPage)}?load={supplier.Identifier}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string id = query["deleted"].ToString();
            SupplierDetailsViewModel supplier = Suppliers.Where((n) => n.Identifier == id).FirstOrDefault();

            if (supplier != null)
                Suppliers.Remove(supplier);
        }
        else if (query.ContainsKey("saved"))
        {
            string id = query["saved"].ToString();
            SupplierDetailsViewModel supplier = Suppliers.Where((n) => n.Identifier == id).FirstOrDefault();

            if (supplier != null)
                supplier.Reload();
            else
                Suppliers.Add(new SupplierDetailsViewModel(suppliers.Get(id)));
        }
    }
}
