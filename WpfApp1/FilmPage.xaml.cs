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
    /// Логика взаимодействия для FilmPage.xaml
    /// </summary>
    public partial class FilmPage : Page
    {
        public Cinema CurrentCinema { get; set; }
        public List<SessionModel> Sessions { get; set; }    
        public FilmPage(Cinema selectedCinema)
        {
            InitializeComponent();

            CurrentCinema = selectedCinema;
            DataContext = this;

            LoadSessions(); 
        }

        private void LoadSessions()
        {
            Sessions = Core.Context.CinemaHall
                    .Where(a => a.CinemaID == CurrentCinema.ID)
                    .Select(a => new SessionModel
                    {
                        Id = a.ID,
                        DateTime = a.DateTime,
                        HallNumber = a.Hall.HallNumber,
                        RatingName = a.Hall.RatingHall.RatingName,
                        ChairPrice = a.Hall.RatingHall.ChairPrice,
                        Description = a.Hall.RatingHall.Description
                    })
                    .OrderBy(a => a.DateTime)
                    .ToList();


            DataContext = null;
            DataContext = this;

            if (Sessions != null && Sessions.Any())
            {
                SessionsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                SessionsListBox.Visibility = Visibility.Collapsed;
            }
        }

        private void SessionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SessionsListBox.SelectedItem is SessionModel selectedSession)
            {
                SessionsListBox.SelectedItem = null;

                if (!CurrentUser.IsAuthenticated)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Для бронирования билетов необходимо войти в систему. Хотите перейти на страницу входа?",
                        "Требуется авторизация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        NavigationService.Navigate(new LoginPage());
                    }
                    return;
                }


                var cinemaHall = Core.Context.CinemaHall
                    .FirstOrDefault(a => a.ID == selectedSession.Id);

                if (cinemaHall != null)
                {
                    cinemaHall.Cinema = Core.Context.Cinema
                        .FirstOrDefault(b => b.ID == cinemaHall.CinemaID);

                    cinemaHall.Hall = Core.Context.Hall
                        .FirstOrDefault(c => c.ID == cinemaHall.HallID);

                    if (cinemaHall.Hall != null)
                    {
                        cinemaHall.Hall.RatingHall = Core.Context.RatingHall
                            .FirstOrDefault(d => d.ID == cinemaHall.Hall.RatingHallID);
                    }

                    NavigationService.Navigate(new SessionPage(cinemaHall));
                }
            }
        }
    }
}
