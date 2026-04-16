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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public Client Client { get; set; }
        public List<Ticket> Tickets { get; set; }

        public ProfilePage()
        {
            InitializeComponent();


            LoadUserData();
            LoadTickets();
            DataContext = this;
        }

        private void LoadUserData()
        {
            Client = Core.Context.Client
                    .FirstOrDefault(c => c.ID == CurrentUser.ClientId.Value);
        }

        private void LoadTickets()
        {
            Tickets = Core.Context.Ticket
                    .Where(t => t.ClientID == CurrentUser.ClientId.Value)
                    .ToList();


            foreach (var ticket in Tickets)
            {
                ticket.CinemaHall = Core.Context.CinemaHall
                    .Include("Cinema")
                    .Include("Hall")
                    .Include("Hall.RatingHall")
                    .FirstOrDefault(ch => ch.ID == ticket.CinemaHallID);

                ticket.Chair = Core.Context.Chair
                    .FirstOrDefault(ch => ch.ID == ticket.ChairID);
            }


            Tickets = Tickets.OrderByDescending(t => t.PurchaseDate).ToList();

            if (Tickets != null && Tickets.Any())
            {
                TicketsListBox.ItemsSource = Tickets;
                TicketsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                TicketsListBox.Visibility = Visibility.Collapsed;
            }


            DataContext = null;
            DataContext = this;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tickets == null) return;

            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    Tickets = Tickets.OrderByDescending(t => t.PurchaseDate).ToList();
                    break;
                case 1:
                    Tickets = Tickets.OrderBy(t => t.PurchaseDate).ToList();
                    break;
                case 2:
                    Tickets = Tickets.OrderBy(t => t.CinemaHall.Cinema.Name).ToList();
                    break;
                case 3:
                    Tickets = Tickets.OrderBy(t => t.Price).ToList();
                    break;
                case 4:
                    Tickets = Tickets.OrderByDescending(t => t.Price).ToList();
                    break;
            }

            TicketsListBox.ItemsSource = null;
            TicketsListBox.ItemsSource = Tickets;
        }

        private void BuyTicketsButton_Click(object sender, RoutedEventArgs e)
        {

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Navigate(new MainPage());
                mainWindow.PageTitleTextBlock.Text = "Главная страница";
                mainWindow.BackButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
