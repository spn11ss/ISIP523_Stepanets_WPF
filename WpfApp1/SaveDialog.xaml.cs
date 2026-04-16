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
    /// Логика взаимодействия для SaveDialog.xaml
    /// </summary>
    public partial class SaveDialog : Page
    {
        private PartsPage _partsPage;

        public SaveDialog(PartsPage partsPage)
        {
            InitializeComponent();
            _partsPage = partsPage;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (string.IsNullOrWhiteSpace(AuthorBox.Text))
            {
                MessageBox.Show("Введите имя автора");
                return;
            }

            _partsPage.SaveAssembly(NameBox.Text, AuthorBox.Text);
            if (NavigationService != null)
                NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
                NavigationService.GoBack();
        }
    }
}