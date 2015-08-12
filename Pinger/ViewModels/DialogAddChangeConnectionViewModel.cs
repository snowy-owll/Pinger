using System;
using System.ComponentModel;

namespace Pinger.ViewModels
{
    class DialogAddChangeConnectionViewModel : ObservableObject
    {
        public DialogAddChangeConnectionViewModel()
        {
            _state = DialogAddChangeConnectionState.Add;
        }

        #region Methods

        private void _connectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((_connection.Name == "") || (_connection.Host == ""))
                CanAccept = false;
            else
                CanAccept = true;
        }

        #endregion

        #region Properties

        private ConnectionViewModel _connection;        
        private DialogAddChangeConnectionState _state;
        private bool _canAccept;

        public ConnectionViewModel Connection
        {
            get { return _connection; }
            set
            {
                if (value != _connection)
                {
                    _connection = value;                    
                    _connection.PropertyChanged += _connectionPropertyChanged;
                    OnPropertyChanged();
                }
            }
        }        

        public DialogAddChangeConnectionState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    if (_state == DialogAddChangeConnectionState.Add)
                        CanAccept = false;
                    else
                        CanAccept = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanAccept
        {
            get { return _canAccept; }
            set
            {
                if (value != _canAccept)
                {
                    _canAccept = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler<RequestCloseDialogEventArgs> ClosingRequest = delegate { };
        private void OnClosingRequest(bool dialogresult)
        {
            _connection.PropertyChanged -= _connectionPropertyChanged;
            ClosingRequest(this, new RequestCloseDialogEventArgs(dialogresult));
        }        
        #endregion

        #region Commands

        private Command _accept = null;
        public Command Accept
        {
            get
            {
                if (_accept == null)
                    _accept = new Command(() =>
                    {
                        OnClosingRequest(true);
                    });
                return _accept;
            }
        }

        private Command _reject = null;
        public Command Reject
        {
            get
            {
                if (_reject == null)
                    _reject = new Command(() =>
                    {
                        OnClosingRequest(false);
                    });
                return _reject;
            }
        }

        #endregion
    }

    enum DialogAddChangeConnectionState
    {
        Add,
        Change
    }

    public class RequestCloseDialogEventArgs : EventArgs
    {
        public RequestCloseDialogEventArgs(bool dialogresult)
        {
            this.DialogResult = dialogresult;
        }

        public bool DialogResult
        {
            get;
            set;
        }
    }
}
