using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Martius.App
{
    public partial class SettingsWindow : Window
    {
        private readonly AppSettings _settings;

        public SettingsWindow(AppSettings settings)
        {
            _settings = settings;
            InitializeComponent();

            MinLengthBox.Text = _settings.MinLeaseMonths.ToString();
            MinLengthBox.Focus();

            DiscountBox.Text = _settings.DiscountPercentage.ToString(CultureInfo.InvariantCulture);
            MinCountBox.Text = _settings.MinLeaseCount.ToString();
        }

        private void SettingsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            _settings.MinLeaseMonths = int.Parse(MinLengthBox.Text);
            decimal.TryParse(DiscountBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var percent);
            _settings.DiscountPercentage = percent;
            _settings.MinLeaseCount = int.Parse(MinCountBox.Text);

            // todo add fallback to default
            SettingsManager.SetUserSettings(_settings);
            Close();
        }
    }
}