using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;
using static Martius.App.InputValidator;

namespace Martius.App
{
    public partial class AddPropertyWindow : Window
    {
        private readonly PropertyService _propertyService;
        public Property CreatedProperty { get; private set; }

        public AddPropertyWindow(PropertyService propertyService)
        {
            _propertyService = propertyService;
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var res = ResChBox.IsChecked ?? false;
            var furn = FurnChBox.IsChecked ?? false;
            var park = ParkChBox.IsChecked ?? false;
            var priceString = $"{RubBox.Text}.{DecimalBox.Text}";

            var address = ParseAddress(CityBox.Text, StreetBox.Text, BuildingBox.Text, AptBox.Text);

            var roomsParsed = int.TryParse(RoomBox.Text, out var roomCount);
            var areaParsed =
                double.TryParse(AreaBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var area);
            var priceParsed =
                decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            if (InputValid(address, roomsParsed, areaParsed, priceParsed) && AmountsValid(roomCount, area, price))
            {
                try
                {
                    CreatedProperty = _propertyService.SaveProperty(address, roomCount, area, res, furn, park, price);
                    Close();
                }
                catch (EntityExistsException ex)
                {
                    DisplayError(ex.Message);
                }
            }
            else
                DisplayError("Одно или несколько полей заполнены неверно.");
        }

        private void DisplayError(string message)
        {
            var caption = "Ошибка при вводе данных";
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;
            MessageBox.Show(message, caption, button, icon);
        }
    }
}