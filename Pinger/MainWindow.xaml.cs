using Pinger.ViewModels;
using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pinger
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
