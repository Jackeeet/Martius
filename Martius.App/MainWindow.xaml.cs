using Martius.AppLogic;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly LeaseService _leaseService;
        private readonly TenantService _tenantService;
        private readonly PropertyService _propertyService;


        public MainWindow()
        {
            _leaseService = new LeaseService();
            _tenantService = new TenantService();
            _propertyService = new PropertyService();
            InitializeComponent();
            LeaseTab.Content = new LeaseControl(_leaseService, _tenantService, _propertyService);
            TenantTab.Content = new TenantControl(_tenantService);
            PropertyTab.Content = new PropertyControl(_propertyService);
        }
    }
}