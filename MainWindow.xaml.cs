using CrazyZoo.viewmodels;
using System.Windows;

namespace CrazyZoo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ZooViewModel();
        }
    }
}
