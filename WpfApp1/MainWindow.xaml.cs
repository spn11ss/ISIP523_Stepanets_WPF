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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Product> products = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Page1_Product());
        }

        private void BProduct_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1_Product());
        }

        private void BKorzina_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2_Korzina());
        }

        private void MainFrame_OnNavigated( object sender, NavigationEventArgs e)
        {
            if (e.Content is Page1_Product)
            {
                Title = "Парфюмерный магазин - Каталог товаров";
            }
            else if (e.Content is Page2_Korzina)
            {
                Title = "Парфюмерный магазин - Корзина";
            }
            else if (e.Content is Page3_Order)
            {
                Title = "Парфюмерный магазин - Оформление заказа";
            }
        }


    }
}
