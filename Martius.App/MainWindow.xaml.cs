using Martius.AppLogic;
using System.Configuration;
using System.Windows;

namespace Martius.App
{
    public partial class MainWindow
    {
        private readonly string _connectionString;
        private readonly SettingsWindow _settingsWindow;
        private readonly AppSettings _appSettings;
        private readonly LeaseService _leaseService;
        private readonly TenantService _tenantService;
        private readonly PropertyService _propertyService;

        public MainWindow()
        {
            _appSettings = SettingsManager.GetUserSettings();
            _connectionString = string.IsNullOrEmpty(_appSettings.UserDatabasePath)
                ? ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
                : $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_appSettings.UserDatabasePath};Integrated Security=True;Connect Timeout=30";
            _settingsWindow = new SettingsWindow(_appSettings);

            _leaseService = new LeaseService(_connectionString);
            _tenantService = new TenantService(_connectionString);
            _propertyService = new PropertyService(_connectionString);

            InitializeComponent();
            SetupControls();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _settingsWindow.Owner = this;
        }

        private void SetupControls()
        {
            LeaseTab.Content = new LeaseControl(_leaseService, _tenantService, _propertyService, _appSettings);
            TenantTab.Content = new TenantControl(_tenantService);
            PropertyTab.Content = new PropertyControl(_propertyService);
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            _settingsWindow.ShowDialog();
        }
    }
}