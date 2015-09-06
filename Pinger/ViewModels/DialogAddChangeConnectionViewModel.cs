using System;
using System.ComponentModel;

namespace Pinger.ViewModels
{
    class DialogAddChangeConnectionViewModel : ObservableObject, IDataErrorInfo
    {
        public DialogAddChangeConnectionViewModel()
        {
            Connection = new ConnectionViewModel();
        }

        public DialogAddChangeConnectionViewModel(ConnectionViewModel connection)
        {
            Connection = connection;
        }

        #region Properties

        private ConnectionViewModel _connection;        
        private DialogAddChangeConnectionState _state;
        private bool _nameIsInvalid;
        private bool _hostIsInvalid;

        public ConnectionViewModel Connection
        {
            get { return _connection; }
            set
            {
                if (value != _connection)
                {
                    _connection = value;
                    if (value.Connection.IsNew())
                        State = DialogAddChangeConnectionState.Add;
                    else
                        State = DialogAddChangeConnectionState.Change;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return Connection.Name; }
            set
            {
                if(value!=Connection.Name)
                {
                    Connection.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Host
        {
            get { return Connection.Host; }
            set
            {
                if (value != Connection.Host)
                {
                    Connection.Host = value;
                    OnPropertyChanged();
                }
            }
        }

        public DialogAddChangeConnectionState State
        {
            get { return _state; }
            private set
            {
                if (value != _state)
                {
                    _state = value;                    
                    OnPropertyChanged();
                }
            }
        }

        public bool NameIsInvalid
        {
            get { return _nameIsInvalid; }
            set
            {
                if (value != _nameIsInvalid)
                {
                    _nameIsInvalid = value;
                    if (value || HostIsInvalid)
                        Accept.CanExecute = false;
                    else
                        Accept.CanExecute = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool HostIsInvalid
        {
            get { return _hostIsInvalid; }
            set
            {
                if (value != _hostIsInvalid)
                {
                    _hostIsInvalid = value;
                    if (value || NameIsInvalid)
                        Accept.CanExecute = false;
                    else
                        Accept.CanExecute = true;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler<RequestCloseDialogEventArgs> ClosingRequest = delegate { };
        private void OnClosingRequest(bool dialogresult)
        {            
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

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string result = null;
                switch(columnName)
                {
                    case "Name":
                        if (Name=="")
                        {
                            result = Localization.Localization.ConnectionNameIsRequired;
                            NameIsInvalid = true;
                        }
                        else
                        {
                            NameIsInvalid = false;
                        }
                        break;
                    case "Host":
                        if (Uri.CheckHostName(Host) == UriHostNameType.Unknown)
                        {
                            result = Localization.Localization.AddressIsInvalid;
                            HostIsInvalid = true;
                        }
                        else
                        {
                            HostIsInvalid = false;
                        }
                        break;
                }
                return result;
            }
        }        
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
