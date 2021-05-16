using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Martius.Domain;

namespace Martius.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LeaseTab.Content = new LeaseControl();
            TenantTab.Content = new TenantControl();
            PropertyTab.Content = new PropertyControl();
        }
    }
}