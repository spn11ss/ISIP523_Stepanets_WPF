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
    /// Логика взаимодействия для Page3_Order.xaml
    /// </summary>
    public partial class Page3_Order : Page
    {

        public Page3_Order()
        {
            InitializeComponent();
            LoudPage();
            Finaly_PriceTxt.Text = $"{Symma()}";
        }

        public void LoudPage()
        {
            Page3LstBx.ItemsSource = MainWindow.products;
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {

                return email.Contains("@") &&
                       email.IndexOf("@") < email.LastIndexOf(".") &&
                       email.LastIndexOf(".") > email.IndexOf("@") + 1;
            }
            catch
            {
                return false;
            }
        }
        private void OrderingButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FIOtxt.Text) || string.IsNullOrWhiteSpace(Emailtxt.Text) || string.IsNullOrWhiteSpace(Adrestxt.Text))
            {
                MessageBox.Show("Введите все данные!!!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!IsValidEmail(Emailtxt.Text))
            {
                MessageBox.Show("Введите корректный Email адрес!\nEmail должен содержать символ @ и точку после него.",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Emailtxt.Focus();
                Emailtxt.SelectAll();
                return;
            }

            var order = new Order
            {
                FIO = FIOtxt.Text,
                Email = Emailtxt.Text,
                Adress = Adrestxt.Text,
                FinalPrice = Decimal.Parse(Finaly_PriceTxt.Text),
            };
            foreach (var product in MainWindow.products)
            {
                order.OrderProduct.Add(new OrderProduct
                {
                    ProductID = product.ID,
                    OrderID= order.ID
   
                });
                
            }

            Core.Context.Order.Add(order);
            Core.Context.SaveChanges();

            

            MessageBox.Show("Заказ оформлен!)");

            MainWindow.products.Clear();

            FIOtxt.Text = "";
            Emailtxt.Text = "";
            Adrestxt.Text = "";
            Finaly_PriceTxt.Text = "0 руб.";
            Page3LstBx.ItemsSource = null;


        }

        private void GoBackBttn_Click(object sender, RoutedEventArgs e)
        { 
            NavigationService?.GoBack();
        }
        private decimal Symma()
        {
            decimal TotalpPrice = 0;
            foreach (Product products in MainWindow.products)
            {
                TotalpPrice += products.Price;
            }
            return TotalpPrice;
        }
    }
}
