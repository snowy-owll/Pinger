using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinger.ViewModels
{
    class ErrorDialogViewModel : ObservableObject
    {
        public ErrorDialogViewModel(string title, string message)
        {
            _title = title;
            _message = message;
        }
        
        #region Properties
        private string _title;
        private string _message;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
