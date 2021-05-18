using System.Windows;
using Martius.AppLogic;
using Martius.Domain;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for NewPropertyWindow.xaml
    /// </summary>
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
            var city = CityBox.Text;
            var street = StreetBox.Text;
            var building = BuildingBox.Text;
            var apartment = AptBox.Text;
            var roomCount = RoomBox.Text;
            var area = AreaBox.Text;
            var res = ResChBox.IsChecked ?? false;
            var furn = FurnChBox.IsChecked ?? false;
            var park = ParkChBox.IsChecked ?? false;
            var price = $"{RubBox.Text}.{DecimalBox.Text}";

            CreatedProperty = _propertyService.SaveProperty(
                city, street, building, apartment, roomCount, area, res, furn, park, price);
            Close();
        }
    }
}