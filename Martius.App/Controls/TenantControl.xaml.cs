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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for TenantControl.xaml
    /// </summary>
    public partial class TenantControl : UserControl
    {

        private AddTenantWindow _newTenant;

        public TenantControl()
        {
            InitializeComponent();
        }

        private void TenantSearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewTenantButton_Click(object sender, RoutedEventArgs e)
        {
            _newTenant = new AddTenantWindow();
            _newTenant.ShowDialog();
        }
    }
}
