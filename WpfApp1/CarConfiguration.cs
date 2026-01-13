using System.Collections.Generic;
using System.ComponentModel;

namespace WpfApp1
{
    public class CarConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _model;
        public string Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }

        private string _engine;
        public string Engine { get => _engine; set { _engine = value; OnPropertyChanged(nameof(Engine)); } }

        private string _color;
        public string Color { get => _color; set { _color = value; OnPropertyChanged(nameof(Color)); } }

        private List<string> _options = new List<string>();
        public List<string> Options { get => _options; set { _options = value; OnPropertyChanged(nameof(Options)); } }

        private decimal _basePrice;
        public decimal BasePrice { get => _basePrice; set { _basePrice = value; OnPropertyChanged(nameof(BasePrice)); } }

        private decimal _enginePrice;
        public decimal EnginePrice { get => _enginePrice; set { _enginePrice = value; OnPropertyChanged(nameof(EnginePrice)); } }

        private decimal _colorPrice;
        public decimal ColorPrice { get => _colorPrice; set { _colorPrice = value; OnPropertyChanged(nameof(ColorPrice)); } }

        private decimal _optionsPrice;
        public decimal OptionsPrice
        {
            get => _optionsPrice;
            set
            {
                _optionsPrice = value;
                OnPropertyChanged(nameof(OptionsPrice));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public decimal TotalPrice => BasePrice + EnginePrice + ColorPrice + OptionsPrice;

        private decimal _downPaymentPercent = 20;
        public decimal DownPaymentPercent { get => _downPaymentPercent; set { _downPaymentPercent = value; OnPropertyChanged(nameof(DownPaymentPercent)); } }

        private int _loanTermMonths = 36;
        public int LoanTermMonths { get => _loanTermMonths; set { _loanTermMonths = value; OnPropertyChanged(nameof(LoanTermMonths)); } }

        private decimal _loanRate = 5;
        public decimal LoanRate { get => _loanRate; set { _loanRate = value; OnPropertyChanged(nameof(LoanRate)); } }

        public decimal DownPayment => TotalPrice * (DownPaymentPercent / 100);
        public decimal LoanAmount => TotalPrice - DownPayment;

        private decimal _monthlyPayment;
        public decimal MonthlyPayment { get => _monthlyPayment; set { _monthlyPayment = value; OnPropertyChanged(nameof(MonthlyPayment)); } }

        private string _customerName;
        public string CustomerName { get => _customerName; set { _customerName = value; OnPropertyChanged(nameof(CustomerName)); } }

        private string _phone;
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(nameof(Phone)); } }

        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); } }

        public void CalculateMonthlyPayment()
        {
            if (LoanAmount <= 0 || LoanTermMonths <= 0)
            {
                MonthlyPayment = 0;
                return;
            }

            decimal monthlyRate = (LoanRate / 12) / 100;
            decimal temp = (decimal)System.Math.Pow((double)(1 + monthlyRate), LoanTermMonths);
            MonthlyPayment = LoanAmount * (monthlyRate * temp) / (temp - 1);
        }
    }

    public static class Config
    {
        public static CarConfig Current { get; set; } = new CarConfig();
    }
}