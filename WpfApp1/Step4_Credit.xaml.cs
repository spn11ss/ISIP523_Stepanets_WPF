using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1
{
    public partial class Step4_Credit : Page
    {
        private bool isPageLoaded = false;

        public Step4_Credit()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isPageLoaded = true;

            if (Config.Current != null)
            {
                Config.Current.CalculateMonthlyPayment();
                UpdateDisplay();
            }
        }

        public void UpdateDisplay()
        {
            if (!isPageLoaded || Config.Current == null) return;

            TotalPriceText.Text = $"{Config.Current.TotalPrice:N0}€";
            DownPaymentSlider.Value = (double)Config.Current.DownPaymentPercent;
            DownPaymentPercentText.Text = $"{Config.Current.DownPaymentPercent}%";
            DownPaymentAmountText.Text = $"{Config.Current.DownPayment:N0}€";
            TermSlider.Value = Config.Current.LoanTermMonths;
            TermText.Text = $"{Config.Current.LoanTermMonths} мес.";
            LoanRateText.Text = $"{Config.Current.LoanRate}% годовых";
            LoanAmountText.Text = $"{Config.Current.LoanAmount:N0}€";
            MonthlyPaymentText.Text = $"{Config.Current.MonthlyPayment:N0}€";
        }

        private void DownPaymentSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isPageLoaded || Config.Current == null) return;

            Config.Current.DownPaymentPercent = (decimal)e.NewValue;
            Config.Current.CalculateMonthlyPayment();
            UpdateDisplay();
        }

        private void TermSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isPageLoaded || Config.Current == null) return;

            Config.Current.LoanTermMonths = (int)e.NewValue;
            Config.Current.CalculateMonthlyPayment();
            UpdateDisplay();
        }
    }
}