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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Cinema> _allCinemas;
        private string _searchText = "";
        private string _currentSort = "";
        public MainPage()
        {
            InitializeComponent();
            LoadCinemas();
        }

        private void LoadCinemas()
        {
            _allCinemas = Core.Context.Cinema.ToList();
            CinemaListBox.ItemsSource = _allCinemas;
        }
        private void AppFilter()
        {
            if (_allCinemas == null) return;

            IEnumerable<Cinema> query = _allCinemas;


            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                query = query.Where(c => c.Name != null &&
                    c.Name.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }


            if (_currentSort == "Name")
            {
                query = query.OrderBy(c => c.Name);
            }
            else if (_currentSort == "Rating")
            {
                query = query.OrderByDescending(c => c.Rating);
            }

            CinemaListBox.ItemsSource = query.ToList();

        }
        private void SortByName_Checked(object sender, RoutedEventArgs e)
        {
            _currentSort = "Name";
            AppFilter();
        }

        private void SortByRating_Checked(object sender, RoutedEventArgs e)
        {
            _currentSort = "Rating";
            AppFilter();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchText = SearchTextBox.Text;
            AppFilter();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            SortByName.IsChecked = false;
            SortByRating.IsChecked = false;
            _currentSort = "";
            AppFilter();
        }

        private void CinemaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CinemaListBox.SelectedItem is Cinema selectedCinema)
            {
                CinemaListBox.SelectedItem = null;
                NavigationService.Navigate(new FilmPage(selectedCinema));
            }
        }
    }
}
