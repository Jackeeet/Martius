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

            MinLengthBox.Focus();
            FillSettingsFields();
        }

        private void SettingsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var monthsParsed = int.TryParse(MinLengthBox.Text, out var months);
            var discountParsed =
                decimal.TryParse(DiscountBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var percent);
            var countParsed = int.TryParse(MinCountBox.Text, out var count);

            if (!(monthsParsed && discountParsed && countParsed))
            {
                var caption = "Ошибка при вводе данных";
                var message = "Одно или несколько полей заполнены некорректно. Установить настройки по умолчанию?";
                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    SettingsManager.SetDefaultSettings();
                    FillSettingsFields();
                }
            }
            else
            {
                _settings.MinLeaseMonths = months;
                _settings.DiscountPercentage = percent;
                _settings.MinLeaseCount = count;
                SettingsManager.SetUserSettings(_settings);
                FillSettingsFields();
            }
        }

        private void FillSettingsFields()
        {
            MinLengthBox.Text = _settings.MinLeaseMonths.ToString();
            DiscountBox.Text = _settings.DiscountPercentage.ToString(CultureInfo.InvariantCulture);
            MinCountBox.Text = _settings.MinLeaseCount.ToString();
        }
    }
}