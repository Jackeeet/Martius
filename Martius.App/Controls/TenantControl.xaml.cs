using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Martius.AppLogic;
using Martius.Domain;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for TenantControl.xaml
    /// </summary>
    public partial class TenantControl : UserControl
    {
        private AddTenantWindow _newTenantWindow;
        private readonly List<Tenant> _allTenants;
        private readonly TenantService _tenantService;

        public TenantControl(TenantService tenantService)
        {
            _tenantService = tenantService;
            _allTenants = _tenantService.AllTenants;
            InitializeComponent();
            TenantListView.ItemsSource = _allTenants;
        }

        private void TenantSearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewTenantButton_Click(object sender, RoutedEventArgs e)
        {
            _newTenantWindow = new AddTenantWindow(_tenantService);
            _newTenantWindow.ShowDialog();
            if (_newTenantWindow.CreatedTenant != null)
            {
                _allTenants.Add(_newTenantWindow.CreatedTenant);
                TenantListView.Items.Refresh();
            }
        }
    }
}