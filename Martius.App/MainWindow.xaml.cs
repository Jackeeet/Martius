namespace Martius.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AddLeaseWindow _newLease;
        private AddTenantWindow _newTenant;
        private AddPropertyWindow _newProp;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void LeaseSearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void NewLeaseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _newLease = new AddLeaseWindow { Owner = this };
            _newLease.ShowDialog();
        }

        private void CurrentChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ExpiredChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void TenantSearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void NewTenantButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _newTenant = new AddTenantWindow { Owner = this };
            _newTenant.ShowDialog();
        }

        private void PropertySearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void NewPropertyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _newProp = new AddPropertyWindow { Owner = this };
            _newProp.ShowDialog();
        }

        private void RentedChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void AvailableChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ResidentialChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void NonResidentialChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void FurnishedChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ParkingChBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void MinAreaTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void MaxAreaTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Rooms1Button_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Rooms2Button_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Rooms3Button_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Rooms4RButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Rooms5RButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

    }
}