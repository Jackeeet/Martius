using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;
using Martius.Domain;
using static System.String;

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
        private GridViewColumnHeader _sortColumn;
        private SortAdorner _sortAdorner;

        public TenantControl(TenantService tenantService)
        {
            _tenantService = tenantService;
            _allTenants = _tenantService.AllTenants;
            InitializeComponent();
            TenantListView.ItemsSource = _allTenants;

            var view = (CollectionView) CollectionViewSource.GetDefaultView(TenantListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            view.Filter = TenantFilter;
        }

        private bool TenantFilter(object obj)
        {
            var searchTerm = SearchBox.Text;
            return IsNullOrEmpty(searchTerm) ||
                   ((Tenant) obj).FullName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void NewTenantButton_Click(object sender, RoutedEventArgs e)
        {
            _newTenantWindow = new AddTenantWindow(_tenantService);
            _newTenantWindow.ShowDialog();
            if (_newTenantWindow.CreatedTenant != null)
                TenantListView.Items.Refresh();
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

        private void TenantSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(TenantListView.ItemsSource).Refresh();
        }
    }
}