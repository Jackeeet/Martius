using System;
using System.Globalization;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for AddLeaseWindow.xaml
    /// </summary>
    public partial class AddLeaseWindow : Window
    {
        private readonly LeaseService _leaseService;
        private readonly TenantService _tenantService;
        private readonly PropertyService _propertyService;
        public Lease CreatedLease { get; private set; }

        public AddLeaseWindow(LeaseService leaseService, TenantService tenantService, PropertyService propertyService)
        {
            _leaseService = leaseService;
            _tenantService = tenantService;
            _propertyService = propertyService;
            InitializeComponent();
            PropertyCBox.ItemsSource = _propertyService.AllProperties;
            TenantCBox.ItemsSource = _tenantService.AllTenants;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var property = (RealProperty) PropertyCBox.SelectedItem;
            var tenant = (Tenant) TenantCBox.SelectedItem;

            var startDate = StartDatePicker.SelectedDate.GetValueOrDefault();
            var endDate = EndDatePicker.SelectedDate.GetValueOrDefault();

            // todo add discount check;
            var price = property.MonthlyPrice;

            CreatedLease = _leaseService.SaveLease(property, tenant, price, startDate, endDate);
            Close();
        }

        private void PropertyCBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var property = (RealProperty) PropertyCBox.SelectedItem;
            var rub = Math.Truncate(property.MonthlyPrice);
            var dec = property.MonthlyPrice % 1.0m;
            RubBox.Text = rub.ToString(CultureInfo.InvariantCulture);
            DecimalBox.Text = dec.ToString(CultureInfo.InvariantCulture);
        }

        private void TenantCBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void DiscountChBox_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}