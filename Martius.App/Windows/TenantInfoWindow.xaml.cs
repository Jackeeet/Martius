using System;
using System.Windows;
using Martius.AppLogic;
using Martius.Domain;
using static Martius.App.TenantInputParser;

namespace Martius.App
{
    public partial class TenantInfoWindow : Window
    {
        private readonly TenantService _service;
        private readonly Tenant _tenant;
        public event Action InfoChanged;

        public TenantInfoWindow(Tenant tenant, TenantService service)
        {
            _tenant = tenant;
            _service = service;
            InitializeComponent();

            IdLabel.Content += _tenant.Id.ToString();
            SurnameBox.Text = _tenant.PersonInfo.Surname;
            NameBox.Text = _tenant.PersonInfo.Name;
            PatrBox.Text = _tenant.PersonInfo.Patronym;
            DobPicker.SelectedDate = _tenant.PersonInfo.DateOfBirth;
            PassBox.Text = _tenant.PassportNumber;
            PhoneBox.Text = _tenant.PhoneNumber;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e) => SetInputState(false);

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dob = DobPicker.SelectedDate.GetValueOrDefault();
            var person = ParsePerson(SurnameBox.Text, NameBox.Text, PatrBox.Text, dob);
            var passport = ParsePassport(PassBox.Text);
            var phone = ParsePhone(PhoneBox.Text);

            if (InputValid(person, passport, phone))
            {
                _tenant.PersonInfo = person;
                _tenant.PassportNumber = passport;
                _tenant.PhoneNumber = phone;
                _service.UpdateTenant(_tenant);
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
            SurnameBox.IsReadOnly = value;
            NameBox.IsReadOnly = value;
            PatrBox.IsReadOnly = value;
            PassBox.IsReadOnly = value;
            PhoneBox.IsReadOnly = value;

            DobPicker.IsEnabled = !value;
            ApplyButton.IsEnabled = !value;
        }
    }
}