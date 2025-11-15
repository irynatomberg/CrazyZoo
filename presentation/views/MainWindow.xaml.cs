using CrazyZoo.presentation.viewmodels;
using System.Windows;

namespace CrazyZoo.presentation.views
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
