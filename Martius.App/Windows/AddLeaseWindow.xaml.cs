using System;
using System.Globalization;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;
using static Martius.Infrastructure.CastUtils;

namespace Martius.App
{
    public partial class AddLeaseWindow : Window
    {
        private LeaseService _leaseService;
        private decimal _discount;
        private decimal _discountAmount = decimal.Zero;
        private int _minLeaseCount;
        private int _minLeaseMonths;
        private string _errCaption = "Ошибка при вводе данных";

        public Lease CreatedLease { get; private set; }

        public AddLeaseWindow(LeaseService leaseService, TenantService tenantService, PropertyService propertyService,
            AppSettings appSettings)
        {
            SetupStructure(leaseService, appSettings);
            InitializeComponent();

            PropertyCBox.ItemsSource = propertyService.Properties;
            TenantCBox.ItemsSource = tenantService.Tenants;
            MonthsBox.Text = _minLeaseMonths.ToString();
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
            var property = (Property)PropertyCBox.SelectedItem;
            var tenant = (Tenant)TenantCBox.SelectedItem;

            var sd = StartDatePicker.SelectedDate.GetValueOrDefault();
            int.TryParse(MonthsBox.Text, out var monthCount);

            var priceString = $"{RubBox.Text}.{DecimalBox.Text}";
            var priceParsed = decimal.TryParse(
                priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            if (InputValid(property, tenant, monthCount, priceParsed))
            {
                var ed = sd.AddMonths(monthCount);
                try
                {
                    CreatedLease = _leaseService.SaveLease(property, tenant, price, sd, ed);
                    Close();
                }
                catch (EntityExistsException ex)
                {
                    DisplayError(ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    DisplayError(ex2.Message);
                }
            }
            else
                DisplayError("Одно или несколько полей заполнены неверно.");
        }

        private bool InputValid(Property prop, Tenant tenant, int months, bool priceParsed)
            => prop != null && tenant != null && months >= _minLeaseMonths && priceParsed;

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
            var tenant = (Tenant)TenantCBox.SelectedItem;
            var prop = (Property)PropertyCBox.SelectedItem;
            _discountAmount = _leaseService.GetDiscountedPrice(prop, tenant, _minLeaseCount, _discount);
            DiscountChBox.IsEnabled = _discountAmount != decimal.Zero;
        }

        private void DiscountChBox_OnClick(object sender, RoutedEventArgs e)
        {
            if (DiscountChBox.IsChecked.GetValueOrDefault())
            {
                var actualPrice = _discountAmount;
                RubBox.Text = Math.Truncate(actualPrice).ToString(CultureInfo.InvariantCulture);
                DecimalBox.Text = GetDecimalPoints(actualPrice).ToString(CultureInfo.InvariantCulture);
            }
            else
                DisplayDefaultPrice();
        }

        private void DisplayDefaultPrice()
        {
            var property = (Property)PropertyCBox.SelectedItem;
            var rub = Math.Truncate(property.MonthlyPrice);
            var dec = GetDecimalPoints(property.MonthlyPrice);
            RubBox.Text = rub.ToString(CultureInfo.InvariantCulture);
            DecimalBox.Text = dec.ToString(CultureInfo.InvariantCulture);
        }

        private void DisplayError(string message)
        {
            MessageBox.Show(message, _errCaption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}