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
    /// Логика взаимодействия для Page1_Product.xaml
    /// </summary>
    public partial class Page1_Product : Page
    {
        private static List<Product> products = new List<Product>();
        List<Product> prod = Core.Context.Product.ToList();



        public Page1_Product(List<Product> products)
        {
            InitializeComponent();
            products = prod;
            LoudPage();

        }
        private void LoudPage()
        {
            Page1LstBx.ItemsSource = prod;
        }
        private void Vkorzinu_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Product selproduct = btn.DataContext as Product;
            if (selproduct == null) return;
        }

        //private void GoToCart_Click(object sender, RoutedEventArgs e)
        //{
        //    // Передаем корзину на страницу корзины
        //    NavigationService.Navigate(new PageCart(CartProducts));
        //}
    }
}
