using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;
using static Martius.App.TenantInputParser;

namespace Martius.App
{
    public partial class AddTenantWindow : Window
    {
        private readonly TenantService _tenantService;
        private string _errCaption = "Ошибка при вводе данных";
        public Tenant CreatedTenant { get; private set; }

        public AddTenantWindow(TenantService tenantService)
        {
            _tenantService = tenantService;
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dob = DobPicker.SelectedDate.GetValueOrDefault();
            var person = ParsePerson(SurnameBox.Text, NameBox.Text, PatronymBox.Text, dob);
            var passport = ParsePassport(PassportBox.Text);
            var phone = ParsePhone(PhoneBox.Text);

            if (InputValid(person, passport, phone))
            {
                try
                {
                    CreatedTenant = _tenantService.SaveTenant(person, passport, phone);
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
            MessageBox.Show(message, _errCaption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}