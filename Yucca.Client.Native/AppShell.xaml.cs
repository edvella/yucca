namespace Yucca.Client.Native
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.SupplierDetailsPage), typeof(Views.SupplierDetailsPage));
        }
    }
}
