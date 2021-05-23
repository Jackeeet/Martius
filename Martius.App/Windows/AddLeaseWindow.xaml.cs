using System;
using System.Globalization;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.App
{
    public partial class AddLeaseWindow : Window
    {
        private LeaseService _leaseService;
        private decimal _discount;
        private decimal _discountAmount = decimal.Zero;
        private int _minLeaseCount;
        private int _minLeaseMonths;

        public Lease CreatedLease { get; private set; }

        public AddLeaseWindow(LeaseService leaseService, TenantService tenantService, PropertyService propertyService,
            AppSettings appSettings)
        {
            SetupStructure(leaseService, appSettings);
            InitializeComponent();

            PropertyCBox.ItemsSource = propertyService.Properties;
            TenantCBox.ItemsSource = tenantService.Tenants;
        }

        private void SetupStructure(LeaseService leaseService, AppSettings appSettings)
        {
            _leaseService = leaseService;
            _discount = appSettings.DiscountPercentage;
            _minLeaseCount = appSettings.MinLeaseCount;
            _minLeaseMonths = appSettings.MinLeaseMonths;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var property = (Property) PropertyCBox.SelectedItem;
            var tenant = (Tenant) TenantCBox.SelectedItem;

            var sd = StartDatePicker.SelectedDate.GetValueOrDefault();
            var ed = EndDatePicker.SelectedDate.GetValueOrDefault();

            var priceString = $"{RubBox.Text}.{DecimalBox.Text}";
            var priceParsed = decimal.TryParse(
                priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            if (InputValid(property, tenant, sd, ed, priceParsed))
            {
                try
                {
                    CreatedLease = _leaseService.SaveLease(property, tenant, price, sd, ed);
                    Close();
                }
                catch (EntityExistsException ex)
                {
                    DisplayError(ex.Message);
                }
                catch (PropertyRentedException ex2)
                {
                    DisplayError(ex2.Message);
                }
            }
            else
                DisplayError("Одно или несколько полей заполнены неверно");
        }

        private bool InputValid(Property prop, Tenant tenant, DateTime sd, DateTime ed, bool parsed)
            => prop != null && tenant != null && (sd.AddMonths(_minLeaseMonths) <= ed) && parsed;

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
                DecimalBox.Text = (actualPrice % 1m).ToString(CultureInfo.InvariantCulture);
            }
            else
                DisplayDefaultPrice();
        }

        private void DisplayDefaultPrice()
        {
            var property = (Property) PropertyCBox.SelectedItem;
            var rub = Math.Truncate(property.MonthlyPrice);
            var dec = property.MonthlyPrice % 1m;
            RubBox.Text = rub.ToString(CultureInfo.InvariantCulture);
            DecimalBox.Text = dec.ToString(CultureInfo.InvariantCulture);
        }

        private void DisplayError(string message)
        {
            var caption = "Ошибка при вводе данных";
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;
            MessageBox.Show(message, caption, button, icon);
        }
    }
}