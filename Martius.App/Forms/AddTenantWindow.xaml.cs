using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Martius.AppLogic;
using Martius.Domain;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for NewTenantWindow.xaml
    /// </summary>
    public partial class AddTenantWindow : Window
    {
        private readonly TenantService _tenantService;
        public Tenant CreatedTenant { get; private set; }

        public AddTenantWindow(TenantService tenantService)
        {
            _tenantService = tenantService;
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var surname = SurnameBox.Text;
            var name = NameBox.Text;
            var patronym = PatronymBox.Text;
            var dob = DobPicker.SelectedDate.GetValueOrDefault();
            var passport = PassportBox.Text;
            var phone = PhoneBox.Text;

            CreatedTenant = _tenantService.SaveTenant(surname, name, patronym, dob, passport, phone);
            Close();
        }
    }
}