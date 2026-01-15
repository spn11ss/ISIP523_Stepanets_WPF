using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1;

namespace WpfApp1
{
    public partial class Step5_Order : Page
    {
        public Step5_Order()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Config.Current == null) return;


            SummaryPanel.Children.Clear();

            SummaryPanel.Children.Add(new TextBlock
            {
                Text = $"Автомобиль: {Config.Current.Model}, {Config.Current.Engine}, {Config.Current.Color}",
                Margin = new Thickness(0, 0, 0, 5)
            });
            SummaryPanel.Children.Add(new TextBlock
            {
                Text = $"Стоимость: {Config.Current.TotalPrice:N0}€",
                Margin = new Thickness(0, 0, 0, 5)
            });
            SummaryPanel.Children.Add(new TextBlock
            {
                Text = $"Ежемесячный платёж: {Config.Current.MonthlyPayment:N0}€",
                Margin = new Thickness(0, 0, 0, 5)
            });


            if (!string.IsNullOrEmpty(Config.Current.CustomerName))
                NameTextBox.Text = Config.Current.CustomerName;

            if (!string.IsNullOrEmpty(Config.Current.Phone))
                PhoneTextBox.Text = Config.Current.Phone;

            if (!string.IsNullOrEmpty(Config.Current.Email))
                EmailTextBox.Text = Config.Current.Email;
        }




        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {

                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }


        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {


            var allowedKeys = new[]
            {
                Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
                Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
                Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9,
                Key.Back, Key.Delete, Key.Tab,
                Key.Left, Key.Right, Key.Up, Key.Down,
                Key.Home, Key.End, Key.Insert,
                Key.Enter, Key.Return, Key.Escape
            };


            if (!allowedKeys.Contains(e.Key))
            {

                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    if (e.Key == Key.C || e.Key == Key.X || e.Key == Key.V || e.Key == Key.A)
                    {
                        return;
                    }
                }
                e.Handled = true;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (PhoneTextBox.IsFocused)
                {
                    if (Clipboard.ContainsText())
                    {
                        string clipboardText = Clipboard.GetText();

                        foreach (char c in clipboardText)
                        {
                            if (!char.IsDigit(c))
                            {
                                e.Handled = true;
                                MessageBox.Show("Можно вставлять только цифры!");
                                return;
                            }
                        }
                    }
                }
            }

            base.OnPreviewKeyDown(e);
        }



        public bool ValidateData()
        {
            if (Config.Current == null) return false;


            Config.Current.CustomerName = NameTextBox.Text.Trim();
            Config.Current.Phone = PhoneTextBox.Text.Trim();
            Config.Current.Email = EmailTextBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(Config.Current.CustomerName))
            {
                MessageBox.Show("Введите имя!");
                NameTextBox.Focus();
                return false;
            }

            if (Config.Current.CustomerName.Length < 2)
            {
                MessageBox.Show("Имя должно содержать не менее 2 символов!");
                NameTextBox.Focus();
                NameTextBox.SelectAll();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Config.Current.Phone))
            {
                MessageBox.Show("Введите номер телефона!");
                PhoneTextBox.Focus();
                return false;
            }


            if (!IsDigitsOnly(Config.Current.Phone))
            {
                MessageBox.Show("Номер телефона должен содержать только цифры!");
                PhoneTextBox.Focus();
                PhoneTextBox.SelectAll();
                return false;
            }

            if (Config.Current.Phone.Length < 10)
            {
                MessageBox.Show("Номер телефона должен содержать не менее 10 цифр!");
                PhoneTextBox.Focus();
                PhoneTextBox.SelectAll();
                return false;
            }


            if (string.IsNullOrWhiteSpace(Config.Current.Email))
            {
                MessageBox.Show("Введите email адрес!");
                EmailTextBox.Focus();
                return false;
            }

            if (!IsEmailValid(Config.Current.Email))
            {
                MessageBox.Show("Введите корректный email адрес!\nПример: example@mail.com");
                EmailTextBox.Focus();
                EmailTextBox.SelectAll();
                return false;
            }


            ShowOrderSummary();
            return true;
        }


        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }


        private bool IsEmailValid(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;


                if (!email.Contains("@") || !email.Contains("."))
                    return false;

                if (email.Length < 5)
                    return false;

                if (email.StartsWith("@") || email.EndsWith("@"))
                    return false;

                if (email.StartsWith(".") || email.EndsWith("."))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ShowOrderSummary()
        {
            string optionsText = Config.Current.Options.Count > 0
                ? string.Join(", ", Config.Current.Options)
                : "нет дополнительных опций";

            string summary =
                "=== ЗАЯВКА ОФОРМЛЕНА ===\n\n" +
                $"Клиент: {Config.Current.CustomerName}\n" +
                $"Телефон: {Config.Current.Phone}\n" +
                $"Email: {Config.Current.Email}\n\n" +
                $"Автомобиль:\n" +
                $"  • Модель: {Config.Current.Model}\n" +
                $"  • Двигатель: {Config.Current.Engine}\n" +
                $"  • Цвет: {Config.Current.Color}\n" +
                $"  • Опции: {optionsText}\n\n" +
                $"Стоимость: {Config.Current.TotalPrice:N0}€\n" +
                $"Ежемесячный платеж: {Config.Current.MonthlyPayment:N0}€";

            MessageBox.Show(summary, "Заявка оформлена", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}