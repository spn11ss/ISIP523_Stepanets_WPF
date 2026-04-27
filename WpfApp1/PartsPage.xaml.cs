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
    /// Логика взаимодействия для PartsPage.xaml
    /// </summary>
    public partial class PartsPage : Page
    {

        private Dictionary<int, basepart> selectedParts = new Dictionary<int, basepart>();

        public PartsPage()
        {
            InitializeComponent();
            Loaded += PartsPage_Loaded;
        }

        private void PartsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var types = Core.Context.parttype.ToList();
            PartTypesList.ItemsSource = types;
            if (types.Count > 0) PartTypesList.SelectedIndex = 0;

            var mans = Core.Context.manufacturer.OrderBy(m => m.name).ToList();
            mans.Insert(0, new manufacturer { id = 0, name = "Все" });
            ManufacturerFilter.ItemsSource = mans;
            ManufacturerFilter.SelectedIndex = 0;
        }

        private void LoadParts()
        {
            if (PartTypesList.SelectedItem == null) return;
            if (ManufacturerFilter.SelectedItem == null) return;

            int typeId = (int)PartTypesList.SelectedValue;
            int manId = (int)ManufacturerFilter.SelectedValue;
            string search = SearchBox.Text?.ToLower() ?? "";

            var query = Core.Context.basepart.Where(p => p.parttypeid == typeId);

            if (manId > 0)
                query = query.Where(p => p.manufacturerid == manId);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.name.ToLower().Contains(search));

            var result = query.Select(p => new { p.id, p.name, p.price }).ToList();
            PartsList.ItemsSource = result;
        }

        private void PartTypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadParts();
        }
        private void ManufacturerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadParts();
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadParts();
        }

        private void PartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PartsList.SelectedItem == null) return;

            dynamic item = PartsList.SelectedItem;
            int partId = item.id;
            int typeId = (int)PartTypesList.SelectedValue;

            var part = Core.Context.basepart.FirstOrDefault(p => p.id == partId);
            if (part == null) return;

            if (selectedParts.ContainsKey(typeId))
            {
                if (MessageBox.Show($"Заменить {selectedParts[typeId].name} на {part.name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    selectedParts[typeId] = part;
            }
            else
            {
                selectedParts.Add(typeId, part);
            }

            UpdateSelectedList();
            UpdateTotalPrice();
            CheckCompatibility();

            PartsList.SelectedItem = null;
        }

        private void RemovePart_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            dynamic item = btn?.Tag;
            var part = item?.Part as basepart;
            if (part != null)
            {
                var key = selectedParts.FirstOrDefault(kv => kv.Value.id == part.id).Key;
                selectedParts.Remove(key);
                UpdateSelectedList();
                UpdateTotalPrice();
                CheckCompatibility();
            }
        }

        private void UpdateSelectedList()
        {
            var list = selectedParts.Values.Select(p => new { Part = p }).ToList();
            SelectedPartsList.ItemsSource = list;
        }

        private void UpdateTotalPrice()
        {
            decimal total = selectedParts.Values.Sum(p => p.price);
            TotalPrice.Text = $"{total:N0} ₽";
        }

        private void CheckCompatibility()
        {
            selectedParts.TryGetValue(1, out var cpu);
            selectedParts.TryGetValue(4, out var mb);
            selectedParts.TryGetValue(3, out var ram);
            selectedParts.TryGetValue(2, out var gpu);
            selectedParts.TryGetValue(5, out var pcCase);
            selectedParts.TryGetValue(6, out var psu);

            var errors = new List<string>();

            if (cpu != null && mb != null)
            {
                var cpuData = Core.Context.cpu.FirstOrDefault(c => c.id == cpu.id);
                var mbData = Core.Context.motherboard.FirstOrDefault(m => m.id == mb.id);
                if (cpuData != null && mbData != null)
                {
                    if (cpuData.socketid != mbData.socketid)
                        errors.Add("Сокет процессора и материнской платы не совместимы");
                }
            }

            if (mb != null && pcCase != null)
            {
                var mbData = Core.Context.motherboard.FirstOrDefault(m => m.id == mb.id);
                if (mbData != null)
                {
                    var compatible = Core.Context.boardformfactorcase.Any(x => x.caseid == pcCase.id && x.formfactorid == mbData.formfactorid);
                    if (!compatible)
                        errors.Add("Форм-фактор не совместим с корпусом");
                }
            }

            if (mb != null && ram != null)
            {
                var mbData = Core.Context.motherboard.FirstOrDefault(m => m.id == mb.id);
                var ramData = Core.Context.ram.FirstOrDefault(r => r.id == ram.id);
                if (mbData != null && ramData != null)
                {
                    if (mbData.memorytypeid != ramData.memorytypeid)
                        errors.Add("Тип памяти не совместим");
                }
            }

            if (psu != null && gpu != null)
            {
                var psData = Core.Context.powersupply.FirstOrDefault(p => p.id == psu.id);
                var gpuData = Core.Context.gpu.FirstOrDefault(g => g.id == gpu.id);
                if (psData != null && gpuData != null && gpuData.recommendpower != null)
                {
                    if (psData.power < gpuData.recommendpower)
                        errors.Add($"Блок питания {psData.power}W недостаточен, требуется {gpuData.recommendpower}W");
                }
            }

            if (errors.Count == 0)
                CompatibilityStatus.Text = "Все компоненты совместимы!";
            else
                CompatibilityStatus.Text = string.Join("\n", errors);
        }

        public void SaveAssembly(string name, string author)
        {
            if (selectedParts.Count == 0)
            {
                MessageBox.Show("Нет выбранных компонентов");
                return;
            }
            var assembly = new assembly { name = name, author = author };
            Core.Context.assembly.Add(assembly);
            Core.Context.SaveChanges();

            foreach (var part in selectedParts.Values)
            {
                Core.Context.partassembly.Add(new partassembly { partid = part.id, assemblyid = assembly.id });
            }
            Core.Context.SaveChanges();

            MessageBox.Show("Сборка сохранена!");

        }
    }
}