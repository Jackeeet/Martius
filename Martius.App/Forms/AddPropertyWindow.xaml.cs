using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.App
{
    public partial class AddPropertyWindow : Window
    {
        private readonly PropertyService _propertyService;
        public Property CreatedProperty { get; private set; }

        private readonly Regex _bldNumRegex = new Regex(@"^(\d{1,5})(\D*)$", RegexOptions.Compiled);
        private readonly Regex _aptNumRegex = new Regex(@"^(\d{1,5})$", RegexOptions.Compiled);

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

            var address = ParseAddress();

            var roomsParsed = int.TryParse(RoomBox.Text, out var roomCount);
            var areaParsed =
                double.TryParse(AreaBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var area);
            var priceParsed =
                decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            if (InputValid(address, roomsParsed, areaParsed, priceParsed) && AmountsValid(roomCount, area, price))
            {
                CreatedProperty = _propertyService.SaveProperty(address, roomCount, area, res, furn, park, price);
                Close();
            }
            else
            {
                DisplayError("Одно или несколько полей заполнены неверно");
            }
        }

        private static bool InputValid(Address address, bool roomsParsed, bool areaParsed, bool priceParsed)
        {
            return address != null && roomsParsed && areaParsed && priceParsed;
        }

        private static bool AmountsValid(int roomCount, double area, decimal price)
        {
            return roomCount > 0 && area > 0.0d && price > decimal.Zero;
        }

        private Address ParseAddress()
        {
            var buildingMatch = _bldNumRegex.Match(BuildingBox.Text);
            var apartmentMatch = _aptNumRegex.Match(AptBox.Text);
            if (!AddressCorrect(buildingMatch, apartmentMatch))
                return null;

            var building = ParseBuildingNumber(buildingMatch);
            var aptNum = ParseApartmentNumber(apartmentMatch);
            return building.Number == 0 || aptNum == 0
                ? null
                : new Address(CityBox.Text, StreetBox.Text, building.Number, aptNum, building.Extra);
        }

        private bool AddressCorrect(Match bld, Match apt)
        {
            var aptParsed = apt.Success || string.IsNullOrEmpty(AptBox.Text);

            return bld.Success && aptParsed &&
                   !string.IsNullOrEmpty(CityBox.Text) && CityBox.Text.Length <= 50 &&
                   !string.IsNullOrEmpty(StreetBox.Text) && StreetBox.Text.Length <= 50;
        }

        private int? ParseApartmentNumber(Match match)
        {
            var numGroup = match.Groups[1].ToString();
            return CastUtils.ToNullableInt(numGroup);
        }

        private BuildingNumber ParseBuildingNumber(Match match)
        {
            var numGroup = match.Groups[1].ToString();
            var buildNum = int.Parse(numGroup);
            var extra = match.Groups[2].ToString();
            return new BuildingNumber(buildNum, extra);
        }

        private void DisplayError(string message)
        {
            var caption = "Ошибка при вводе данных";
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;
            MessageBox.Show(message, caption, button, icon);
        }

        private class BuildingNumber
        {
            public readonly int Number;
            public readonly string Extra;

            public BuildingNumber(int num, string extra)
            {
                Number = num;
                Extra = extra;
            }
        }
    }
}