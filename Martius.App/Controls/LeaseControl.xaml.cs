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
    /// Interaction logic for LeaseControl.xaml
    /// </summary>
    public partial class LeaseControl : UserControl
    {
        private AddLeaseWindow _newLease;

        public LeaseControl()
        {
            InitializeComponent();
        }

        private void LeaseSearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NewLeaseButton_Click(object sender, RoutedEventArgs e)
        {
            _newLease = new AddLeaseWindow();
            _newLease.ShowDialog();
        }

        private void CurrentChBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ExpiredChBox_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}
