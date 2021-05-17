using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Martius.AppLogic;
using Martius.Domain;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for LeaseControl.xaml
    /// </summary>
    public partial class LeaseControl : UserControl
    {
        private readonly LeaseService _leaseService;
        private readonly TenantService _tenantService;
        private readonly PropertyService _propertyService;
        private AddLeaseWindow _newLeaseWindow;
        private readonly List<Lease> _allLeases;

        public LeaseControl(LeaseService leaseService, TenantService tenantService, PropertyService propertyService)
        {
            _leaseService = leaseService;
            _tenantService = tenantService;
            _propertyService = propertyService;
            _allLeases = _leaseService.AllLeases;
            InitializeComponent();
            LeaseListView.ItemsSource = _allLeases;
            TenantCBox.ItemsSource = _tenantService.AllPeople;
            CityCBox.ItemsSource = _propertyService.AllCities;
        }

        private void LeaseSearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewLeaseButton_Click(object sender, RoutedEventArgs e)
        {
            _newLeaseWindow = new AddLeaseWindow(_leaseService, _tenantService, _propertyService);
            _newLeaseWindow.ShowDialog();
            if (_newLeaseWindow.CreatedLease != null)
            {
                _allLeases.Add(_newLeaseWindow.CreatedLease);
                LeaseListView.Items.Refresh();
            }
        }

        private void CurrentChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ExpiredChBox_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}