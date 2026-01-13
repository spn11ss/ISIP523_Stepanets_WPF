using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Step1_Model());
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPage = MainFrame.Content as Page;


            if (currentPage is Step5_Order)
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите вернуться назад?\nВсе введенные данные на этой странице будут потеряны.",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    ResetStep5Data();


                    if (MainFrame.CanGoBack)
                        MainFrame.GoBack();
                }

            }
            else
            {

                if (MainFrame.CanGoBack)
                    MainFrame.GoBack();
            }
        }


        private void ResetStep5Data()
        {
            if (Config.Current == null) return;


            Config.Current.CustomerName = "";
            Config.Current.Phone = "";
            Config.Current.Email = "";
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPage = MainFrame.Content as Page;

            if (currentPage is Step1_Model)
            {
                if (string.IsNullOrEmpty(Config.Current.Model) || string.IsNullOrEmpty(Config.Current.Engine))
                {
                    MessageBox.Show("Выберите модель и тип двигателя!");
                    return;
                }
                MainFrame.Navigate(new Step2_ColorAndOptions());
            }
            else if (currentPage is Step2_ColorAndOptions)
            {
                if (string.IsNullOrEmpty(Config.Current.Color))
                {
                    MessageBox.Show("Выберите цвет автомобиля!");
                    return;
                }
                MainFrame.Navigate(new Step3_Summary());
            }
            else if (currentPage is Step3_Summary)
            {
                MainFrame.Navigate(new Step4_Credit());
            }
            else if (currentPage is Step4_Credit)
            {
                MainFrame.Navigate(new Step5_Order());
            }
            else if (currentPage is Step5_Order)
            {
                var orderPage = currentPage as Step5_Order;
                if (orderPage.ValidateData())
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            BackButton.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;

            if (e.Content is Step1_Model)
                ProgressBar.Value = 1;
            else if (e.Content is Step2_ColorAndOptions)
                ProgressBar.Value = 2;
            else if (e.Content is Step3_Summary)
            {
                ProgressBar.Value = 3;
                var summaryPage = e.Content as Step3_Summary;
                summaryPage?.UpdateSummary();
            }
            else if (e.Content is Step4_Credit)
            {
                ProgressBar.Value = 4;
                var creditPage = e.Content as Step4_Credit;
                creditPage?.UpdateDisplay();
            }
            else if (e.Content is Step5_Order)
            {
                ProgressBar.Value = 5;
                NextButton.Content = "Оформить заявку";
            }
            else
            {
                NextButton.Content = "Далее";
            }
        }
    }
}