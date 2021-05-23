using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;

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

            var allLeases = _leaseService.AllLeases;
            LeaseListView.ItemsSource = allLeases;
            TenantCBox.ItemsSource = _tenantService.AllPeople;
            CityCBox.ItemsSource = _propertyService.AllCities;

            _view = (CollectionView) CollectionViewSource.GetDefaultView(LeaseListView.ItemsSource);
            _view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        private void NewLeaseButton_Click(object sender, RoutedEventArgs e)
        {
            _newLeaseWindow = new AddLeaseWindow(_leaseService, _tenantService, _propertyService, _appSettings);
            _newLeaseWindow.ShowDialog();
            if (_newLeaseWindow.CreatedLease != null)
                _view.Refresh();
        }

        private void CurrentChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ExpiredChBox_Checked(object sender, RoutedEventArgs e)
        {
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