using Martius.Domain;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Martius.AppLogic;
using static Martius.Infrastructure.CastUtils;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for PropertyControl.xaml
    /// </summary>
    public partial class PropertyControl : UserControl
    {
        private readonly PropertyService _propertyService;
        private AddPropertyWindow _newPropWindow;
        private readonly List<RealProperty> _allProps;

        private bool _residential = true;
        private bool _nonResidential = true;
        private bool _furnished = true;
        private bool _notFurnished = true;
        private bool _parking = true;
        private bool _noParking = true;

        public PropertyControl(PropertyService propertyService)
        {
            _propertyService = propertyService;
            _allProps = _propertyService.AllProperties;
            InitializeComponent();
            PropertyListView.ItemsSource = _allProps;
            PropertyCityCBox.ItemsSource = _propertyService.AllCities;
        }


        private void PropertySearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            _newPropWindow = new AddPropertyWindow(_propertyService);
            _newPropWindow.ShowDialog();
            if (_newPropWindow.CreatedProperty != null)
            {
                _allProps.Add(_newPropWindow.CreatedProperty);
                PropertyListView.Items.Refresh();
            }
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
    }
}