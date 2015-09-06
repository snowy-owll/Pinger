using Pinger.ViewModels;
using System.Windows;

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
}
