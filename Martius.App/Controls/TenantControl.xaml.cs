using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;

namespace Martius.App
{
    public partial class TenantControl : UserControl
    {
        private AddTenantWindow _newTenantWindow;
        private readonly TenantService _tenantService;
        private GridViewColumnHeader _sortColumn;
        private SortAdorner _sortAdorner;
        private readonly CollectionView _view;
        private string _filter;

        public TenantControl(TenantService tenantService)
        {
            _tenantService = tenantService;
            InitializeComponent();

            var allTenants = _tenantService.Tenants;
            TenantListView.ItemsSource = allTenants;

            _view = (CollectionView) CollectionViewSource.GetDefaultView(TenantListView.ItemsSource);
            _view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SetFilter();
            var list = _tenantService.GetFilteredTenants(_filter);
            TenantListView.ItemsSource = list;
            _view.Refresh();
        }

        private void SetFilter()
        {
            var checkedRb = SearchTerms.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked.GetValueOrDefault());
            if (checkedRb?.Name == "NameRb")
            {
                _filter =
                    $"surname like N'%{SearchBox.Text}%' or name like N'%{SearchBox.Text}%' or patronym like N'%{SearchBox.Text}%'";
            }
            else if (checkedRb?.Name == "PhoneRb")
                _filter = $"phone like '%{SearchBox.Text}%'";
            else
                _filter = $"passport like '%{SearchBox.Text}%'";
        }

        private void NewTenantButton_Click(object sender, RoutedEventArgs e)
        {
            _newTenantWindow = new AddTenantWindow(_tenantService) {Owner = Window.GetWindow(this)};
            _newTenantWindow.ShowDialog();

            if (_newTenantWindow.CreatedTenant != null)
                _view.Refresh();
        }

        private void ColumnHeader_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is GridViewColumnHeader column)
            {
                var sortCriteria = column.Tag.ToString();
                if (_sortColumn != null)
                {
                    AdornerLayer.GetAdornerLayer(_sortColumn)?.Remove(_sortAdorner);
                    TenantListView.Items.SortDescriptions.Clear();
                }

                var newSortDirection = ListSortDirection.Ascending;
                if (_sortColumn == column && _sortAdorner.SortDirection == newSortDirection)
                    newSortDirection = ListSortDirection.Descending;

                _sortColumn = column;
                _sortAdorner = new SortAdorner(_sortColumn, newSortDirection);
                AdornerLayer.GetAdornerLayer(_sortColumn)?.Add(_sortAdorner);
                TenantListView.Items.SortDescriptions.Add(new SortDescription(sortCriteria, newSortDirection));
            }
        }
    }
}