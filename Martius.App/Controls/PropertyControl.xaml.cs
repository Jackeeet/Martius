using System;
using Martius.Domain;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for PropertyControl.xaml
    /// </summary>
    public partial class PropertyControl : UserControl
    {
        private readonly PropertyService _propertyService;
        private AddPropertyWindow _newPropWindow;
        private readonly List<Property> _allProps;
        private List<Property> _currentProps;
        private GridViewColumnHeader _sortColumn;
        private SortAdorner _sortAdorner;

        public PropertyControl(PropertyService propertyService)
        {
            _propertyService = propertyService;
            _allProps = _propertyService.AllProperties;
            InitializeComponent();
            PropertyListView.ItemsSource = _allProps;
            PropertyCityCBox.ItemsSource = _propertyService.AllCities;
            _currentProps = new List<Property>(_allProps);

            var view = (CollectionView) CollectionViewSource.GetDefaultView(PropertyListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        private void NewPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            _newPropWindow = new AddPropertyWindow(_propertyService);
            _newPropWindow.ShowDialog();
            if (_newPropWindow.CreatedProperty != null)
                PropertyListView.Items.Refresh();
        }

        // private void ApplySelection(bool chBoxState, Func<Property, bool> selection)
        // {
        //     if (chBoxState)
        //     {
        //         var matching = _allProps.Where(selection);
        //         _currentProps.AddRange(matching);
        //     }
        //     else
        //     {
        //         var predicate = new Predicate<Property>(selection);
        //         var _ = _currentProps.RemoveAll(predicate);
        //     }
        //
        //     PropertyListView.ItemsSource = _currentProps;
        //     PropertyListView.Items.Refresh();
        // }

        private void ApplySelection(bool chBoxState, Func<Property, bool> selection)
        {
            var res = ResidentialChBox.IsChecked.GetValueOrDefault();
            var nonRes = NonResidentialChBox.IsChecked.GetValueOrDefault();
            var furn = FurnishedChBox.IsChecked.GetValueOrDefault();
            var notFurn = NotFurnishedChBox.IsChecked.GetValueOrDefault();

            if (!res)
            {
                _currentProps.RemoveAll(p => p.IsResidential);
            }

            if (!nonRes)
            {
                _currentProps.RemoveAll(p => !p.IsResidential);
            }

            if (!furn)
            {
                _currentProps.RemoveAll(p => p.IsFurnished);
            }

            if (!notFurn)
            {
                _currentProps.RemoveAll(p => p.IsFurnished);
            }


            // if (chBoxState)
            // {
            //     var matching = _allProps.Where(selection);
            //     _currentProps.AddRange(matching);
            // }
            // else
            // {
            //     var predicate = new Predicate<Property>(selection);
            //     var _ = _currentProps.RemoveAll(predicate);
            // }

            PropertyListView.ItemsSource = _currentProps;
            PropertyListView.Items.Refresh();
        }

        private void RentedChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void AvailableChBox_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ResidentialChBox_OnClick(object sender, RoutedEventArgs e)
        {
            ApplySelection(ResidentialChBox.IsChecked.GetValueOrDefault(), p => p.IsResidential);
        }

        private void NonResidentialChBox_OnClick(object sender, RoutedEventArgs e)
        {
            ApplySelection(NonResidentialChBox.IsChecked.GetValueOrDefault(), p => !p.IsResidential);
        }

        private void FurnishedChBox_OnClick(object sender, RoutedEventArgs e)
        {
            ApplySelection(FurnishedChBox.IsChecked.GetValueOrDefault(), p => p.IsFurnished);
        }

        private void NotFurnishedChBox_OnClick(object sender, RoutedEventArgs e)
        {
            ApplySelection(NotFurnishedChBox.IsChecked.GetValueOrDefault(), p => !p.IsFurnished);
        }

        private void ParkingChBox_OnClick(object sender, RoutedEventArgs e)
        {
            ApplySelection(ParkingChBox.IsChecked.GetValueOrDefault(), p => p.HasParking);
        }

        private void NoParkingChBox_OnClick(object sender, RoutedEventArgs e)
        {
            ApplySelection(NoParkingChBox.IsChecked.GetValueOrDefault(), p => !p.HasParking);
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