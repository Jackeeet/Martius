using System;
using System.Text.RegularExpressions;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.App
{
    public partial class AddTenantWindow : Window
    {
        private readonly TenantService _tenantService;
        public Tenant CreatedTenant { get; private set; }

        private readonly Regex _passportRegex = new Regex(@"^\d{4}[ -]?\d{6}$", RegexOptions.Compiled);

        private readonly Regex _phoneRegex =
            new Regex(@"^\+?\d[ -]?\(?\d{3}\)?[ -]?\d{3}[ -]?\d{2}[ -]?\d{2}$", RegexOptions.Compiled);

        public AddTenantWindow(TenantService tenantService)
        {
            _tenantService = tenantService;
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var person = ParsePerson();
            var passport = ParsePassport();
            var phone = ParsePhone();

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
                DisplayError("Одно или несколько полей заполнены неверно");
        }

        private bool InputValid(Person person, string passport, string phone)
            => person != null && passport != null && phone != null;

        private string ParsePhone()
        {
            var match = _phoneRegex.Match(PhoneBox.Text);
            if (!match.Success)
                return null;

            var phone = match.Groups[0].ToString();
            return Regex.Replace(phone, @"[ \-\(\)]", string.Empty);
        }

        private string ParsePassport()
        {
            var match = _passportRegex.Match(PassportBox.Text);
            if (!match.Success)
                return null;

            var passport = match.Groups[0].ToString();
            return Regex.Replace(passport, @"[ \-]", string.Empty);
        }

        private Person ParsePerson()
        {
            var surname = SurnameBox.Text;
            var name = NameBox.Text;
            var patronym = PatronymBox.Text;
            if (string.IsNullOrEmpty(surname) || surname.Length > 50 ||
                string.IsNullOrEmpty(name) || name.Length > 50 ||
                patronym.Length > 50)
            {
                return null;
            }

            var dob = DobPicker.SelectedDate.GetValueOrDefault();
            if (DateTime.Now.AddYears(-14) < dob)
                return null;

            return new Person(surname, name, patronym, dob);
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