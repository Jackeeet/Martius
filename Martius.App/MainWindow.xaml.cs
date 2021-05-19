using Martius.AppLogic;
using System.Configuration;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private decimal _discount = new decimal(0.95);


        public MainWindow()
        {
            var leaseService = new LeaseService(_connectionString);
            var tenantService = new TenantService(_connectionString);
            var propertyService = new PropertyService(_connectionString);
            InitializeComponent();
            LeaseTab.Content = new LeaseControl(leaseService, tenantService, propertyService, _discount);
            TenantTab.Content = new TenantControl(tenantService);
            PropertyTab.Content = new PropertyControl(propertyService);
        }
    }
}