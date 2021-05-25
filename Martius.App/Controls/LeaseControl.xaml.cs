using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.App
{
    public partial class LeaseControl : UserControl
    {
        private readonly LeaseService _leaseService;
        private readonly TenantService _tenantService;
        private readonly PropertyService _propertyService;
        private AddLeaseWindow _newLeaseWindow;
        private GridViewColumnHeader _sortColumn;
        private SortAdorner _sortAdorner;
        private readonly AppSettings _appSettings;
        private readonly CollectionView _view;

        public LeaseControl(LeaseService leaseService, TenantService tenantService, PropertyService propertyService,
            AppSettings appSettings)
        {
            _leaseService = leaseService;
            _tenantService = tenantService;
            _propertyService = propertyService;
            _appSettings = appSettings;
            InitializeComponent();

            var allLeases = _leaseService.Leases;
            LeaseListView.ItemsSource = allLeases;
            TenantCBox.ItemsSource = _tenantService.Tenants;
            CityCBox.ItemsSource = _propertyService.AllCities;

            _view = (CollectionView) CollectionViewSource.GetDefaultView(LeaseListView.ItemsSource);
            _view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        private void NewLeaseButton_Click(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            _newLeaseWindow = new AddLeaseWindow(_leaseService, _tenantService, _propertyService, _appSettings);
            _newLeaseWindow.Owner = Window.GetWindow(this);
            _newLeaseWindow.ShowDialog();

            if (_newLeaseWindow.CreatedLease != null)
                _view.Refresh();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentChBox.IsChecked = null;
            CurrentChBox.Content = "текущие/истекшие";
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            TenantCBox.SelectedIndex = -1;
            CityCBox.SelectedIndex = -1;

            LeaseListView.ItemsSource = _leaseService.Leases;
            _view.Refresh();
            ResetButton.IsEnabled = false;
        }

        private void RentedChBox_OnClick(object sender, RoutedEventArgs e)
        {
            OnFilterChanged(sender, e);
            CurrentChBox.Content = CurrentChBox.IsChecked == null ? "текущие/истекшие" :
                CurrentChBox.IsChecked == true ? "текущие" : "истекшие";
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (ResetButton.IsEnabled == false)
                ResetButton.IsEnabled = true;

            var join = " inner join property on property_id = property.id inner join tenant on tenant_id = tenant.id ";
            LeaseListView.ItemsSource = _leaseService.GetFilteredLeases(BuildFilter(), join);
            _view.Refresh();
        }

        private string BuildFilter()
        {
            var today = CastUtils.FormatSqlDate(DateTime.Now);
            var current = "null";
            var expired = "null";
            if (CurrentChBox.IsChecked == true)
                current = "1";
            else if (CurrentChBox.IsChecked == false)
                expired = "1";

            var sd = string.IsNullOrEmpty(StartDatePicker.Text)
                ? "null"
                : CastUtils.FormatSqlDate(Convert.ToDateTime(StartDatePicker.Text));
            var ed = string.IsNullOrEmpty(EndDatePicker.Text)
                ? "null"
                : CastUtils.FormatSqlDate(Convert.ToDateTime(EndDatePicker.Text));

            var city = CityCBox.SelectedIndex == -1 ? "null" : $"N'{CityCBox.SelectedItem}'";
            var tId = "null";
            if (TenantCBox.SelectedIndex != -1)
            {
                tId = (Tenant) TenantCBox.SelectedItem == null
                    ? "null"
                    : ((Tenant) TenantCBox.SelectedItem).Id.ToString();
            }

            return
                $"(({current} is null) or end_date >= {today}) and " +
                $"(({expired} is null) or end_date < {today}) and " +
                $"(({sd} is null) or start_date = {sd}) and " +
                $"(({ed} is null) or end_date = {ed}) and " +
                $"(({city} is null) or property.city = {city}) and " +
                $"(({tId} is null) or tenant.id = {tId})";
        }

        private void ColumnHeader_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is GridViewColumnHeader column)
            {
                var sortCriteria = column.Tag.ToString();
                if (_sortColumn != null)
                {
                    AdornerLayer.GetAdornerLayer(_sortColumn)?.Remove(_sortAdorner);
                    LeaseListView.Items.SortDescriptions.Clear();
                }

                var newSortDirection = ListSortDirection.Ascending;
                if (_sortColumn == column && _sortAdorner.SortDirection == newSortDirection)
                    newSortDirection = ListSortDirection.Descending;

                _sortColumn = column;
                _sortAdorner = new SortAdorner(_sortColumn, newSortDirection);
                AdornerLayer.GetAdornerLayer(_sortColumn)?.Add(_sortAdorner);
                LeaseListView.Items.SortDescriptions.Add(new SortDescription(sortCriteria, newSortDirection));
            }
        }
    }
}