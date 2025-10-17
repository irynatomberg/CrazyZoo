using CrazyZoo.entity;
using CrazyZoo.interfaces;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrazyZoo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        private ObservableCollection<Animal> animals = new ObservableCollection<Animal>();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            AnimalList.ItemsSource = animals;

            animals.Add(new Cat { Name = "Muri", Age = 3 });
            animals.Add(new Dog { Name = "Pontu", Age = 5 });
            animals.Add(new Bird { Name = "Tibu", Age = 1 });
        }

        public static void Log(string message)
        {
            if (Instance != null)
            {
                Instance.LogBox.AppendText(message + "\n");
                Instance.LogBox.ScrollToEnd();
            }
        }

        private void AnimalList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal selected)
            {
                NameText.Text = selected.Name;
                AgeText.Text = selected.Age.ToString();
            }
            else
            {
                NameText.Text = "";
                AgeText.Text = "";
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            string type = (TypeCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();
            string name = string.IsNullOrWhiteSpace(NameInput.Text) ? "Nimetu" : NameInput.Text;
            int age = int.TryParse(AgeInput.Text, out int parsed) ? parsed : 1;

            Animal newAnimal = type switch
            {
                "Kass" => new Cat { Name = name, Age = age },
                "Koer" => new Dog { Name = name, Age = age },
                "Lind" => new Bird { Name = name, Age = age },
                _ => new Cat { Name = name, Age = age }
            };

            animals.Add(newAnimal);
            Log($"{newAnimal.Name} ({type}) lisatud.");
            NameInput.Clear();
            AgeInput.Clear();
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal selected)
            {
                animals.Remove(selected);
                Log($"{selected.Name} eemaldatud.");
                NameText.Text = "";
                AgeText.Text = "";
            }
        }

        private void MakeSound_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal selected)
                Log($"{selected.Name}: {selected.MakeSound()}");
        }

        private void Feed_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal selected && !string.IsNullOrWhiteSpace(FoodInput.Text))
                Log($"{selected.Name} sõi {FoodInput.Text}.");
        }

        private void CrazyAction_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is ICrazyAction crazy)
            {
                crazy.ActCrazy();
            }
            else
            {
                Log("See loom ei tee hullu trikki 😅");
            }
        }
    }
}