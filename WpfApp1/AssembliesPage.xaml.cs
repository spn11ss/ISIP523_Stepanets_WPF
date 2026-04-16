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
    /// Логика взаимодействия для AssembliesPage.xaml
    /// </summary>
    public partial class AssembliesPage : Page
    {
        public AssembliesPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                try
                {
                    AssembliesList.ItemsSource = Core.Context.assembly.OrderByDescending(a => a.id).ToList();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            };
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var assembly = btn?.Tag as assembly;
                if (assembly == null) return;

                var parts = Core.Context.partassembly.Where(p => p.assemblyid == assembly.id).Select(p => p.basepart).ToList();
                decimal total = parts.Sum(p => p.price);

                string text = $"{assembly.name}\nАвтор: {assembly.author}\n\nКомпоненты:\n";
                foreach (var p in parts)
                    text += $"- {p.name}: {p.price:C}\n";
                text += $"\nИтого: {total:C}";

                MessageBox.Show(text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
