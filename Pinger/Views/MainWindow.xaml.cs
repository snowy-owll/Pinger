using Pinger.ViewModels;
using System.Windows;

namespace Pinger.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {                
        public MainWindow()
        {            
            InitializeComponent();            
            _model = new MainViewModel();
            DataContext = _model;
            _model.ClosingRequest += (s, e) => Close();            
        }

        private MainViewModel _model;
    }    
}
