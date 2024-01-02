using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Yucca.Inventory;
using Yucca.Volatile;

namespace Yucca.Client.Native.ViewModels;

internal class SupplierDetailsViewModel : ObservableObject, IQueryAttributable
{
    private Supplier _supplier;

    private InMemorySupplierList _supplierList;

    public string Name
    {
        get => _supplier.Name;
        set
        {
            if (_supplier.Name != value)
            {
                _supplier.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public string AddressLine1
    {
        get => _supplier.AddressLine1;
        set
        {
            if (_supplier.AddressLine1 != value)
            {
                _supplier.AddressLine1 = value;
                OnPropertyChanged();
            }
        }
    }

    public string AddressLine2
    {
        get => _supplier.AddressLine2;
        set
        {
            if (_supplier.AddressLine2 != value)
            {
                _supplier.AddressLine2 = value;
                OnPropertyChanged();
            }
        }
    }

    public string City
    {
        get => _supplier.City;
        set
        {
            if (_supplier.City != value)
            {
                _supplier.City = value;
                OnPropertyChanged();
            }
        }
    }

    public string State
    {
        get => _supplier.State;
        set
        {
            if (_supplier.State != value)
            {
                _supplier.State = value;
                OnPropertyChanged();
            }
        }
    }

    public string PostCode
    {
        get => _supplier.PostCode;
        set
        {
            if (_supplier.PostCode != value)
            {
                _supplier.PostCode = value;
                OnPropertyChanged();
            }
        }
    }

    public string Country
    {
        get => _supplier.Country;
        set
        {
            if (_supplier.Country != value)
            {
                _supplier.Country = value;
                OnPropertyChanged();
            }
        }
    }

    public string ContactPhone
    {
        get => _supplier.ContactPhone;
        set
        {
            if (_supplier.ContactPhone != value)
            {
                _supplier.ContactPhone = value;
                OnPropertyChanged();
            }
        }
    }

    public string Email
    {
        get => _supplier.Email;
        set
        {
            if (_supplier.Email != value)
            {
                _supplier.Email = value;
                OnPropertyChanged();
            }
        }
    }

    public string Website
    {
        get => _supplier.Website;
        set
        {
            if (_supplier.Website != value)
            {
                _supplier.Website = value;
                OnPropertyChanged();
            }
        }
    }

    public string TaxNumber
    {
        get => _supplier.TaxNumber;
        set
        {
            if (_supplier.TaxNumber != value)
            {
                _supplier.TaxNumber = value;
                OnPropertyChanged();
            }
        }
    }

    public string Identifier => _supplier.Id;

    public ICommand SaveCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }

    public SupplierDetailsViewModel()
    {
        _supplier = new Supplier();
        _supplierList = new InMemorySupplierList();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public SupplierDetailsViewModel(Supplier supplier)
    {
        _supplier = supplier;
        _supplierList = new InMemorySupplierList();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _supplier.Id = _supplierList.Save(_supplier);
        await Shell.Current.GoToAsync($"..?saved={_supplier.Id}");
    }

    private async Task Delete()
    {
        _supplierList.Remove(_supplier.Id);
        await Shell.Current.GoToAsync($"..?deleted={_supplier.Id}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _supplier = _supplierList.Get(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _supplier = _supplierList.Get(_supplier.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(AddressLine1));
        OnPropertyChanged(nameof(AddressLine2));
        OnPropertyChanged(nameof(City));
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(PostCode));
        OnPropertyChanged(nameof(Country));
        OnPropertyChanged(nameof(ContactPhone));
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(Website));
        OnPropertyChanged(nameof(TaxNumber));
    }
}
