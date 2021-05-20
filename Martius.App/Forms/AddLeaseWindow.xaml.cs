using System;
using System.Globalization;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;

namespace Martius.App
{
    public partial class AddLeaseWindow : Window
    {
        private LeaseService _leaseService;
        private decimal _discount;
        private decimal _discountAmount = decimal.Zero;
        private int _minLeaseCount;

        public Lease CreatedLease { get; private set; }

        public AddLeaseWindow(LeaseService leaseService, TenantService tenantService, PropertyService propertyService,
            AppSettings appSettings)
        {
            SetupStructure(leaseService, appSettings);
            InitializeComponent();

            PropertyCBox.ItemsSource = propertyService.AllProperties;
            TenantCBox.ItemsSource = tenantService.AllTenants;
        }

        private void SetupStructure(LeaseService leaseService, AppSettings appSettings)
        {
            _leaseService = leaseService;
            _discount = appSettings.DiscountPercentage;
            _minLeaseCount = appSettings.MinLeaseCount;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var property = (Property) PropertyCBox.SelectedItem;
            var tenant = (Tenant) TenantCBox.SelectedItem;

            var startDate = StartDatePicker.SelectedDate.GetValueOrDefault();
            var endDate = EndDatePicker.SelectedDate.GetValueOrDefault();

            var priceString = $"{RubBox.Text}.{DecimalBox.Text}";
            decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            CreatedLease = _leaseService.SaveLease(property, tenant, price, startDate, endDate);
            Close();
        }

        private void PropertyCBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DisplayDefaultPrice();
            if (TenantCBox.SelectedIndex != -1)
                CheckEnableDcb();
        }

        private void TenantCBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PropertyCBox.SelectedIndex != -1)
                CheckEnableDcb();
        }

        private void CheckEnableDcb()
        {
            var tenant = (Tenant) TenantCBox.SelectedItem;
            var prop = (Property) PropertyCBox.SelectedItem;
            _discountAmount = _leaseService.GetDiscountedAmount(prop, tenant, _minLeaseCount, _discount);
            DiscountChBox.IsEnabled = _discountAmount != decimal.Zero;
        }

        private void DiscountChBox_OnClick(object sender, RoutedEventArgs e)
        {
            if (DiscountChBox.IsChecked.GetValueOrDefault())
            {
                var actualPrice = _discountAmount;
                RubBox.Text = Math.Truncate(actualPrice).ToString(CultureInfo.InvariantCulture);
                DecimalBox.Text = (actualPrice % 1.0m).ToString(CultureInfo.InvariantCulture);
            }
            else
                DisplayDefaultPrice();
        }

        private void DisplayDefaultPrice()
        {
            var property = (Property) PropertyCBox.SelectedItem;
            var rub = Math.Truncate(property.MonthlyPrice);
            var dec = property.MonthlyPrice % 1.0m;
            RubBox.Text = rub.ToString(CultureInfo.InvariantCulture);
            DecimalBox.Text = dec.ToString(CultureInfo.InvariantCulture);
        }
    }
}