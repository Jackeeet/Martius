using Martius.AppLogic;
using System.Configuration;

namespace Martius.App
{
    public partial class MainWindow
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private AppSettings _appSettings;
        private LeaseService _leaseService;
        private TenantService _tenantService;
        private PropertyService _propertyService;

        public MainWindow()
        {
            SetupStructure();
            InitializeComponent();
            SetupControls();
        }

        private void SetupStructure()
        {
            _leaseService = new LeaseService(_connectionString);
            _tenantService = new TenantService(_connectionString);
            _propertyService = new PropertyService(_connectionString);
            _appSettings = SettingsManager.GetUserSettings();
        }

        private void SetupControls()
        {
            LeaseTab.Content = new LeaseControl(_leaseService, _tenantService, _propertyService, _appSettings);
            TenantTab.Content = new TenantControl(_tenantService);
            PropertyTab.Content = new PropertyControl(_propertyService);
        }
    }
}