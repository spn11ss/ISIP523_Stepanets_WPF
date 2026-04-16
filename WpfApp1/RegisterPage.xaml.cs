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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string login = LoginTextBox.Text.Trim();
            string mail = MailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (!mail.Contains("@") || !mail.Contains("."))
            {
                MessageBox.Show("Введите корректный email адрес!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ConfirmPasswordBox.Password = "";
                return;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать минимум 3 символа!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var existingLogin = Core.Context.Client
                    .Where(c => c.Login == login)
                    .FirstOrDefault();
            if (existingLogin != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var existingMail = Core.Context.Client
                .Where(c => c.Mail == mail)
                .FirstOrDefault();
            if (existingMail != null)
            {
                MessageBox.Show("Пользователь с такой почтой уже зарегистрирован!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var newClient = new Client
            {
                Name = name,
                Login = login,
                Mail = mail,
                Password = password
            };

            Core.Context.Client.Add(newClient);
            Core.Context.SaveChanges();

            CurrentUser.ClientId = newClient.ID;
            CurrentUser.ClientLogin = newClient.Login;
            CurrentUser.ClientName = newClient.Name;
            CurrentUser.ClientMail = newClient.Mail;

            MessageBox.Show($"Регистрация успешно завершена!\nДобро пожаловать, {newClient.Name}!",
                "Регистрация завершена", MessageBoxButton.OK, MessageBoxImage.Information);

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.UpdateAuthUI();
            }

            NavigationService.Navigate(new MainPage());

        }

        private void RegisterLinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
