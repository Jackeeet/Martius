using System.ComponentModel;
using System.Globalization;
using System.Windows;
using Microsoft.Win32;

namespace Martius.App
{
    public partial class SettingsWindow : Window
    {
        private AppSettings _settings;
        private string _filePath;
        private string _defaultDbMsg = "Встроенная база данных";
        private string _errCaption = "Ошибка при вводе данных";
        private string _errMsg = "Одно или несколько полей заполнены некорректно. Установить настройки по умолчанию?";

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
            var countParsed = int.TryParse(MinCountBox.Text, out var minLeaseCount);

            if (!(monthsParsed && discountParsed && countParsed && AmountsValid(months, percent, minLeaseCount)))
            {
                if (MessageBox.Show(_errMsg, _errCaption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    _settings = SettingsManager.SetDefaultSettings();
                }
            }
            else
            {
                UpdateSettings(months, percent, minLeaseCount);
                MessageBox.Show("Изменения сохранены.");
            }

            FillSettingsFields();
        }

        private void UpdateSettings(int months, decimal percent, int minLeaseCount)
        {
            _settings.MinLeaseMonths = months;
            _settings.DiscountPercentage = percent;
            _settings.MinLeaseCount = minLeaseCount;
            _settings.UserDatabasePath = _filePath;
            SettingsManager.SetUserSettings(_settings);
        }

        private bool AmountsValid(int months, decimal discount, int leaseCount)
        {
            return months >= 1 && discount > 0 && discount < 100 && leaseCount >= 0;
        }

        private void FillSettingsFields()
        {
            MinLengthBox.Text = _settings.MinLeaseMonths.ToString();
            DiscountBox.Text = _settings.DiscountPercentage.ToString(CultureInfo.InvariantCulture);
            MinCountBox.Text = _settings.MinLeaseCount.ToString();
            FileBox.Text = string.IsNullOrEmpty(_settings.UserDatabasePath)
                ? _defaultDbMsg
                : _settings.UserDatabasePath;
        }

        private void DbButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog {Filter = "*.mdf|*.mdf"};
            if (fileDialog.ShowDialog() == true)
                _filePath = fileDialog.FileName;
            FileBox.Text = _filePath;
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            _filePath = "";
            FileBox.Text = _defaultDbMsg;
        }
    }
}