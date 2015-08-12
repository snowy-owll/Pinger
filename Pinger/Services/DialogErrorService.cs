using Pinger.Interfaces;
using Pinger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pinger.Services
{
    class DialogErrorService : IDialogService
    {
        public bool? ShowDialog(object datacontext)
        {
            ErrorDialogViewModel model = datacontext as ErrorDialogViewModel;
            MessageBox.Show(model.Message, model.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            return true;
        }
    }
}
