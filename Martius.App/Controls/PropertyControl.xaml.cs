using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.App
{
    public partial class PropertyControl : UserControl
    {
        private readonly PropertyService _propertyService;
        private AddPropertyWindow _newPropWindow;
        private GridViewColumnHeader _sortColumn;
        private SortAdorner _sortAdorner;
        private readonly CollectionView _view;
        private const string SqlTrue = "1";
        private const string SqlFalse = "0";
        private const string SqlNull = "null";

        public PropertyControl(PropertyService propertyService)
        {
            _propertyService = propertyService;
            InitializeComponent();

            PropertyListView.ItemsSource = _propertyService.Properties;
            PropCityCBox.ItemsSource = _propertyService.AllCities;

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

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            PropCityCBox.SelectedIndex = -1;
            ResetChBoxes();
            MinAreaTextBox.Text = "";
            MaxAreaTextBox.Text = "";
            RoomsAnyButton.IsChecked = true;

            PropertyListView.ItemsSource = _propertyService.Properties;
            _view.Refresh();
            ResetButton.IsEnabled = false;
        }

        private void ResetChBoxes()
        {
            foreach (CheckBox option in PropStates.Children)
                option.IsChecked = null;
            foreach (CheckBox option in PropTypes.Children)
                option.IsChecked = null;
            RentedChBox.Content = "сданные/свободные";
            ResidentialChBox.Content = "жилые/нежилые";
            FurnishedChBox.Content = "с мебелью/без мебели";
            ParkingChBox.Content = "с парковкой/без парковки";
        }

        private string BuildFilter()
        {
            var city = PropCityCBox.SelectedIndex == -1 ? SqlNull : $"N'{PropCityCBox.SelectedItem}'";
            var today = CastUtils.GetSqlRepresentation(DateTime.Now);

            var rented = SqlNull;
            var available = SqlNull;
            if (RentedChBox.IsChecked == true)
                rented = SqlTrue;
            else if (RentedChBox.IsChecked == false)
                available = SqlTrue;

            var res = ResidentialChBox.IsChecked == null ? SqlNull :
                ResidentialChBox.IsChecked == true ? SqlTrue : SqlFalse;
            var furn = FurnishedChBox.IsChecked == null ? SqlNull :
                FurnishedChBox.IsChecked == true ? SqlTrue : SqlFalse;
            var park = ParkingChBox.IsChecked == null ? SqlNull :
                ParkingChBox.IsChecked == true ? SqlTrue : SqlFalse;

            var minArea = string.IsNullOrEmpty(MinAreaTextBox.Text) ? SqlNull : MinAreaTextBox.Text;
            var maxArea = string.IsNullOrEmpty(MaxAreaTextBox.Text) ? SqlNull : MaxAreaTextBox.Text;

            var checkedRb = RoomsCount.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked.GetValueOrDefault());
            var rooms = BuildRoomCountString(checkedRb);
            var maxRooms = checkedRb?.Name == "Rooms5RButton" ? "5" : SqlNull;

            return
                $"(({city} is null) or city = {city}) and " +
                $"(({rented} is null) or lease.end_date >= {today}) and " +
                $"(({available} is null) or (lease.end_date < {today} or lease.id is null)) and " +
                $"(({res} is null) or residential = {res}) and " +
                $"(({furn} is null) or furnished = {furn}) and " +
                $"(({park} is null) or has_parking = {park}) and " +
                $"(({minArea} is null) or area >= {minArea}) and " +
                $"(({maxArea} is null) or area <= {maxArea}) and " +
                $"(({rooms} is null) or room_count = {rooms}) and " +
                $"(({maxRooms} is null) or room_count >= {maxRooms})";
        }

        private string BuildRoomCountString(RadioButton checkedRb)
        {
            if (checkedRb?.Name == "RoomsAnyButton" || checkedRb?.Name == "Rooms5RButton")
                return SqlNull;
            return checkedRb?.Content.ToString();
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (ResetButton.IsEnabled == false)
                ResetButton.IsEnabled = true;

            var join = " left join lease on property.id = property_id";
            PropertyListView.ItemsSource =
                _propertyService.GetFilteredProperties(BuildFilter(), join).Distinct().ToList();
            _view.Refresh();
        }

        private void RentedChBox_OnClick(object sender, RoutedEventArgs e)
        {
            OnFilterChanged(sender, e);
            RentedChBox.Content = RentedChBox.IsChecked == null ? "сданные/свободные" :
                RentedChBox.IsChecked == true ? "сданные" : "свободные";
        }

        private void ResidentialChBox_OnClick(object sender, RoutedEventArgs e)
        {
            OnFilterChanged(sender, e);
            ResidentialChBox.Content = ResidentialChBox.IsChecked == null ? "жилые/нежилые" :
                ResidentialChBox.IsChecked == true ? "жилые" : "нежилые";
        }

        private void FurnishedChBox_OnClick(object sender, RoutedEventArgs e)
        {
            OnFilterChanged(sender, e);
            FurnishedChBox.Content = FurnishedChBox.IsChecked == null ? "с мебелью/без мебели" :
                FurnishedChBox.IsChecked == true ? "с мебелью" : "без мебели";
        }

        private void ParkingChBox_OnClick(object sender, RoutedEventArgs e)
        {
            OnFilterChanged(sender, e);
            ParkingChBox.Content = ParkingChBox.IsChecked == null ? "с парковкой/без парковки" :
                ParkingChBox.IsChecked == true ? "с парковкой" : "без парковки";
        }

        private void InfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement) sender).DataContext is Property prop)
            {
                var info = new PropertyInfoWindow(prop, _propertyService) {Owner = Window.GetWindow(this)};
                info.InfoChanged += OnInfoChanged;
                info.Show();
            }
        }

        private void OnInfoChanged() => _view.Refresh();

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