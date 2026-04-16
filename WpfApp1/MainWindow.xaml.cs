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
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PartsPage());
        }

        private void SaveAssembly_Click(object sender, RoutedEventArgs e)
        {
            var page = MainFrame.Content as PartsPage;
            if (page != null)
                MainFrame.Navigate(new SaveDialog(page));
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();

        private void ShowParts_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new PartsPage());

        private void ShowAssemblies_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new AssembliesPage());
    }
}