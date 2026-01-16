using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1
{
    public partial class Step1_Model : Page
    {
        public Step1_Model()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Config.Current.Model))
            {
                foreach (ComboBoxItem item in ModelComboBox.Items)
                {
                    if (item.Content.ToString().StartsWith(Config.Current.Model.Split(' ')[0]))
                    {
                        ModelComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            switch (Config.Current.Engine)
            {
                case "Бензиновый": PetrolEngine.IsChecked = true; break;
                case "Дизельный": DieselEngine.IsChecked = true; break;
                case "Электрический": ElectricEngine.IsChecked = true; break;
            }
        }

        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModelComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                Config.Current.Model = selectedItem.Content.ToString().Split(' ')[0];
                Config.Current.BasePrice = decimal.Parse(selectedItem.Tag.ToString());
            }
        }

        private void Engine_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Config.Current.Engine = rb.Content.ToString().Split(' ')[0];

            switch (Config.Current.Engine)
            {
                case "Бензиновый": Config.Current.EnginePrice = 0; break;
                case "Дизельный": Config.Current.EnginePrice = 2000; break;
                case "Электрический": Config.Current.EnginePrice = 5000; break;
            }
        }
    }
}