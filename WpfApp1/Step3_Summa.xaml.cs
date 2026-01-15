using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1
{
    public partial class Step3_Summa : Page
    {
        public Step3_Summa()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSummary();
        }

        public void UpdateSummary()
        {
            if (Config.Current == null) return;

            ModelText.Text = $"Модель: {Config.Current.Model ?? "не выбрана"}";
            EngineText.Text = $"Двигатель: {Config.Current.Engine ?? "не выбран"}";
            ColorText.Text = $"Цвет: {Config.Current.Color ?? "не выбран"}";
            OptionsText.Text = $"Опции: {string.Join(", ", Config.Current.Options)}";

            BasePriceText.Text = $"Базовая цена модели: {Config.Current.BasePrice:N0}€";
            EnginePriceText.Text = $"Доплата за двигатель: {Config.Current.EnginePrice:N0}€";
            ColorPriceText.Text = $"Доплата за цвет: {Config.Current.ColorPrice:N0}€";
            OptionsPriceText.Text = $"Дополнительные опции: {Config.Current.OptionsPrice:N0}€";

            TotalPriceText.Text = $"Итоговая стоимость: {Config.Current.TotalPrice:N0}€";
        }
    }
}