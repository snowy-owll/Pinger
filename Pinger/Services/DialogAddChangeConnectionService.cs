using Pinger.Interfaces;
using Pinger.Views;
using System.Windows;

namespace Pinger.Services
{
    class DialogAddChangeConnectionService : IDialogService
    {
        public bool? ShowDialog(object datacontext)
        {
            DialogAddChangeConnection dialog = new DialogAddChangeConnection();
            dialog.Owner = Application.Current.MainWindow;
            dialog.DataContext = datacontext;
            return dialog.ShowDialog();
        }
    }
}
