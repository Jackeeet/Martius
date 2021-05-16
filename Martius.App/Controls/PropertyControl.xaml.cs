using Martius.Domain;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Martius.AppLogic;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for PropertyControl.xaml
    /// </summary>
    public partial class PropertyControl : UserControl
    {
        private AddPropertyWindow _newProp;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private List<RealProperty> _properties;
        private readonly PropertyService _propertyService = new PropertyService();

        public PropertyControl()
        {
            InitializeComponent();
            RefreshItemList();
        }

        private void RefreshItemList()
        {
            PropertyListView.Items.Clear();
            _properties = _propertyService.GetAllProperties();
            foreach (var prop in _properties)
                PropertyListView.Items.Add(GenerateListItem(prop));
        }

        private ListViewItem GenerateListItem(RealProperty property)
        {
            return new ListViewItem
            {
                Content = property.ToString()
            };
        }

        private void PropertySearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            _newProp = new AddPropertyWindow(_propertyService);
            _newProp.ShowDialog();
            if (_newProp.InputResult)
                RefreshItemList();
        }

        private void RentedChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void AvailableChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ResidentialChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void NonResidentialChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void FurnishedChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ParkingChBox_Checked(object sender, RoutedEventArgs e)
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
    }
}