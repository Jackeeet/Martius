using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;

namespace Martius.App
{
    public partial class PropertyControl : UserControl
    {
        private readonly PropertyService _propertyService;
        private AddPropertyWindow _newPropWindow;

        // private readonly List<Property> _allProps;
        private GridViewColumnHeader _sortColumn;
        private SortAdorner _sortAdorner;
        private readonly CollectionView _view;

        public PropertyControl(PropertyService propertyService)
        {
            _propertyService = propertyService;
            InitializeComponent();

            var allProps = _propertyService.Properties;
            PropertyListView.ItemsSource = allProps;
            PropertyCityCBox.ItemsSource = _propertyService.AllCities;

            _view = (CollectionView) CollectionViewSource.GetDefaultView(PropertyListView.ItemsSource);
            _view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        private void NewPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            _newPropWindow = new AddPropertyWindow(_propertyService) {Owner = Window.GetWindow(this)};
            _newPropWindow.ShowDialog();
            
            if (_newPropWindow.CreatedProperty != null)
                _view.Refresh();
        }

        private void RentedChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void AvailableChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ResidentialChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void NonResidentialChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void FurnishedChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void NotFurnishedChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ParkingChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void NoParkingChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void MinAreaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void MaxAreaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Rooms1Button_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Rooms2Button_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Rooms3Button_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Rooms4RButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Rooms5RButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void PropertyCityCBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ResidentialChBox_OnChecked(object sender, RoutedEventArgs e)
        {
        }

        private void ResidentialChBox_OnUnchecked(object sender, RoutedEventArgs e)
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
                    PropertyListView.Items.SortDescriptions.Clear();
                }

                var newSortDirection = ListSortDirection.Ascending;
                if (_sortColumn == column && _sortAdorner.SortDirection == newSortDirection)
                    newSortDirection = ListSortDirection.Descending;

                _sortColumn = column;
                _sortAdorner = new SortAdorner(_sortColumn, newSortDirection);
                AdornerLayer.GetAdornerLayer(_sortColumn)?.Add(_sortAdorner);
                PropertyListView.Items.SortDescriptions.Add(new SortDescription(sortCriteria, newSortDirection));
            }
        }
    }
}