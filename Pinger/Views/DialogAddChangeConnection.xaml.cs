using Pinger.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Pinger.Views
{
    /// <summary>
    /// Interaction logic for DialogAddChangeConnection.xaml
    /// </summary>    
    public partial class DialogAddChangeConnection : Window
    {
        public DialogAddChangeConnection()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
                {
                    DialogAddChangeConnectionViewModel model = e.NewValue as DialogAddChangeConnectionViewModel;
                    if (model == null)
                        return;
                    model.ClosingRequest += model_ClosingRequest;
                    _model = model;
                };
        }

        DialogAddChangeConnectionViewModel _model;

        void model_ClosingRequest(object sender, RequestCloseDialogEventArgs e)
        {
            if (_model != null)
                _model.ClosingRequest -= model_ClosingRequest;
            DialogResult = e.DialogResult;
            Close();
        }
    }

    class Attached : DependencyObject
    {
        public static readonly DependencyProperty TitleObjectProperty =
            DependencyProperty.RegisterAttached("TitleObject", typeof(object), typeof(Attached),
            new PropertyMetadata((object)null));

        public static object GetTitleObject(DependencyObject d)
        {
            return (object)d.GetValue(TitleObjectProperty);
        }

        public static void SetTitleObject(DependencyObject d, object value)
        {
            d.SetValue(TitleObjectProperty, value);
        }
    }
}
