using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1
{
    public partial class Step2_Specifications : Page
    {
        public Step2_Specifications()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Config.Current.Color) && ColorPanel != null)
            {
                foreach (var child in ColorPanel.Children)
                {
                    if (child is RadioButton rb)
                    {
                        // Извлекаем название цвета без цены
                        string colorName = GetOptionName(rb.Content.ToString());
                        if (colorName == Config.Current.Color)
                        {
                            rb.IsChecked = true;
                            break;
                        }
                    }
                }
            }
        }

        private void Color_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            string fullText = rb.Content.ToString();
            string colorName = GetOptionName(fullText);
            Config.Current.Color = colorName;

            switch (colorName)
            {
                case "Белый": Config.Current.ColorPrice = 0; break;
                case "Чёрный": Config.Current.ColorPrice = 500; break;
                case "Серый": Config.Current.ColorPrice = 800; break;
                case "Красный": Config.Current.ColorPrice = 1000; break;
                case "Синий": Config.Current.ColorPrice = 1200; break;
            }
        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string fullText = cb.Content.ToString();
            string optionName = GetOptionName(fullText);

            if (!Config.Current.Options.Contains(optionName))
            {
                Config.Current.Options.Add(optionName);
                UpdateOptionsPrice();
            }
        }

        private void Option_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string fullText = cb.Content.ToString();
            string optionName = GetOptionName(fullText);

            Config.Current.Options.Remove(optionName);
            UpdateOptionsPrice();
        }

        private string GetOptionName(string fullText)
        {

            int bracketIndex = fullText.IndexOf(" (+");
            if (bracketIndex > 0)
            {
                return fullText.Substring(0, bracketIndex).Trim();
            }
            return fullText;
        }

        private void UpdateOptionsPrice()
        {
            decimal price = 0;
            foreach (string option in Config.Current.Options)
            {
                switch (option)
                {
                    case "Кожаный салон": price += 2000; break;
                    case "Панорамная крыша": price += 1500; break;
                    case "Подогрев сидений": price += 400; break;
                }
            }
            Config.Current.OptionsPrice = price;
        }
    }
}