using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Martius.AppLogic;
using Martius.Domain;
using static Martius.App.PropInputParser;
using static Martius.Infrastructure.CastUtils;

namespace Martius.App
{
    public partial class PropertyInfoWindow : Window
    {
        private readonly PropertyService _service;
        private readonly Property _prop;
        public event Action InfoChanged;

        public PropertyInfoWindow(Property prop, PropertyService service)
        {
            _prop = prop;
            _service = service;
            InitializeComponent();
            DataContext = _prop;
            InitFields(_prop);
        }

        private void InitFields(Property prop)
        {
            IdLabel.Content += prop.Id.ToString();
            CityBox.Text = prop.Address.City;
            StreetBox.Text = prop.Address.Street;
            BuildingBox.Text = prop.Address.Building;
            AptBox.Text = prop.Address.ApartmentNumber.ToString();
            RubBox.Text = Math.Truncate(prop.MonthlyPrice).ToString(CultureInfo.InvariantCulture);
            DecBox.Text = GetDecimalPoints(prop.MonthlyPrice).ToString(CultureInfo.InvariantCulture);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e) => SetInputState(false);

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            var res = ResChBox.IsChecked ?? false;
            var furn = FurnChBox.IsChecked ?? false;
            var park = ParkChBox.IsChecked ?? false;
            var priceString = $"{RubBox.Text}.{DecBox.Text}";

            var address = ParseAddress(CityBox.Text, StreetBox.Text, BuildingBox.Text, AptBox.Text);
            var roomsParsed = int.TryParse(RoomBox.Text, out var roomCount);
            var areaParsed =
                double.TryParse(AreaBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var area);
            var priceParsed =
                decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            if (PropInputValid(address, roomsParsed, areaParsed, priceParsed) &&
                PropAmountsValid(roomCount, area, price))
            {
                _prop.Address = address;
                _prop.RoomCount = roomCount;
                _prop.Area = area;
                _prop.IsResidential = res;
                _prop.IsFurnished = furn;
                _prop.HasParking = park;
                _prop.MonthlyPrice = price;
                _service.UpdateProperty(_prop);
                MessageBox.Show("Изменения сохранены.");
                InfoChanged?.Invoke();

                SetInputState(true);
            }
            else
                MessageBox.Show("Одно или несколько полей заполнены неверно.", "Ошибка при вводе данных",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SetInputState(bool value)
        {
            CityBox.IsReadOnly = value;
            StreetBox.IsReadOnly = value;
            BuildingBox.IsReadOnly = value;
            AptBox.IsReadOnly = value;
            AreaBox.IsReadOnly = value;
            RoomBox.IsReadOnly = value;
            RubBox.IsReadOnly = value;
            DecBox.IsReadOnly = value;
            foreach (CheckBox box in ChBoxes.Children)
                box.IsEnabled = !value;
            ApplyButton.IsEnabled = !value;
        }
    }
}