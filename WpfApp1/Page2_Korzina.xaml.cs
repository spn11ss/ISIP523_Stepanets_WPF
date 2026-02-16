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
    /// Логика взаимодействия для Page2_Korzina.xaml
    /// </summary>
    public partial class Page2_Korzina : Page
    {
        public Page2_Korzina()
        {
            InitializeComponent();
            LoudPage();
        }

        private void LoudPage()
        {
            Page2LstBx.ItemsSource = MainWindow.products;
            Final_PriceTxt.Text = $"{Summary()}";
        }
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            if (but == null) return;

            Product productik = but.DataContext as Product;
            if(productik != null)
            {
                MainWindow.products.Remove(productik);
                Page2LstBx.ItemsSource = null;
                Page2LstBx.ItemsSource = MainWindow.products;
                Summary();

            }
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            Page3_Order orderpage = new Page3_Order();
            NavigationService?.Navigate(orderpage);
        }
        private decimal Summary()
        {
            decimal total = 0;
            foreach(Product products in MainWindow.products)
            {
                total += products.Price;
            }
            return total;
        }
    }
}
