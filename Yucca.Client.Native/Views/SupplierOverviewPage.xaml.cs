namespace Yucca.Client.Native.Views;

public partial class SupplierOverviewPage : ContentPage
{
	public SupplierOverviewPage()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		supplierCollection.SelectedItem = null;
    }
}