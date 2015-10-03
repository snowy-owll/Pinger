using Pinger.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using System.Diagnostics;
using System.Linq;
using Pinger.DB;
using System.ComponentModel;

namespace Pinger.ViewModels
{
    class MainViewModel : ObservableObject, IDataErrorInfo
    {
        public MainViewModel()
        {
            _oldConnections = new OldConnectionsViewModel(_settings);
            _connections = new ConnectionsViewModel(_settings);
            _settingsViewModel = new SettingsViewModel(_settings);
            _listReplies.CollectionChanged += (s, e) =>
            {
                string replies = "";
                foreach (int item in _listReplies)
                {
                    if (item != -1)
                    {
                        replies += item.ToString() + Localization.Localization.Ping_Microseconds + "\n";
                        
                    }
                    else
                    {
                        replies += Localization.Localization.Ping_RequestTimedOut + "\n";
                    }
                }
                Replies = replies;
            };                                                
            _pingExecuted = false;
            _currentOldConnectionIsInvalid = false;
            CurrentPingState = PingState.PingStopped;                                   
        }

        private Settings _settings = new Settings();
        private ObservableCollection<int> _listReplies = new ObservableCollection<int>();
        private PingThread _pingThread = null;
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;        

        #region Methods

        private void _pingReplyReceived(object sender, PingReplyReceivedEventArgs e)
        {
            _dispatcher.BeginInvoke(new Action(() =>
            {
                int t = 0;
                if (e.PingReply.Status == IPStatus.Success)
                    t = (int)e.PingReply.RoundtripTime;
                else
                    t = -1;                                
                _listReplies.Add(t);
                if (_listReplies.Count > _settings.MaxRepliesCount)
                    _listReplies.RemoveAt(0);
                if (!_listReplies.Contains(-1))
                {
                    CurrentPingState = PingState.GoodSignal;
                    if(_settings.SoundPing==SoundPing.SignalIsGood)
                        System.Media.SystemSounds.Beep.Play();
                }
                else
                {
                    int i = 0;
                    foreach (int item in _listReplies)
                    {
                        if (item == -1) i++;
                    }
                    if (i == _listReplies.Count)
                    {
                        CurrentPingState = PingState.NoSignal;
                        if (_settings.SoundPing == SoundPing.NoSignal)
                            System.Media.SystemSounds.Beep.Play();
                    }
                    else
                        CurrentPingState = PingState.BadSignal;
                }
            }));
        }

        private void _pingStopped(object sender, PingStoppedEventArgs e)
        {
            _dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.ReasonStopPing == ReasonStopPing.ExceptionStop)
                {
                    PingExecuted = false;
                    ErrorDialogViewModel model = new ErrorDialogViewModel(Localization.Localization.Error, Localization.Localization.Ping_CouldNotFindHost + 
                        " \"" + (sender as PingThread).Connection.Host + "\"");
                    new DialogErrorService().ShowDialog(model);
                }                
            }));
        }

        #endregion

        #region Properties

        private int _currentTab;
        private int _currentOldConnectionIndex;
        private ConnectionViewModel _currentConnection;
        private string _currentOldConnectionText;
        private bool _currentOldConnectionIsInvalid;
        private ConnectionViewModel _currentOldConnection;
        private PingState _pingState = PingState.PingStopped;
        private OldConnectionsViewModel _oldConnections;
        private ConnectionsViewModel _connections;
        private SettingsViewModel _settingsViewModel;
        private string _replies;        
        private bool _pingExecuted;                             

        public int CurrentTab
        {
            get { return _currentTab; }
            set
            {
                if (value != _currentTab)
                {
                    _currentTab = value;
                    if (_currentTab == 0)
                    {
                        if (CurrentOldConnectionIsInvalid)
                            ExecutePing.CanExecute = false;
                        else
                            ExecutePing.CanExecute = true;
                    }
                    else
                    {
                        if (CurrentConnection == null)
                            ExecutePing.CanExecute = false;
                        else
                            ExecutePing.CanExecute = true;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentOldConnectionIndex
        {
            get { return _currentOldConnectionIndex; }
            set
            {
                if (value != _currentOldConnectionIndex)
                {
                    _currentOldConnectionIndex = value;                    
                    OnPropertyChanged();
                }
            }
        }

        public ConnectionViewModel CurrentConnection
        {
            get { return _currentConnection; }
            set
            {
                if (value != _currentConnection)
                {
                    _currentConnection = value;
                    if (_currentConnection == null && CurrentTab == 1)
                        ExecutePing.CanExecute = false;
                    else if (_currentConnection != null && CurrentTab == 1)
                        ExecutePing.CanExecute = true;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentOldConnectionText
        {
            get { return _currentOldConnectionText; }
            set
            {
                if (value != _currentOldConnectionText)
                {
                    _currentOldConnectionText = value;                    
                    OnPropertyChanged();
                }
            }
        }

        public bool CurrentOldConnectionIsInvalid
        {
            get { return _currentOldConnectionIsInvalid; }
            set
            {
                if (value != _currentOldConnectionIsInvalid)
                {
                    _currentOldConnectionIsInvalid = value;
                    if (_currentOldConnectionIsInvalid && CurrentTab == 0)
                        ExecutePing.CanExecute = false;
                    else if (!_currentOldConnectionIsInvalid && CurrentTab == 0)
                        ExecutePing.CanExecute = true;
                    OnPropertyChanged();
                }
            }
        }

        public ConnectionViewModel CurrentOldConnection
        {
            get { return _currentOldConnection; }
            set
            {
                if (value != _currentOldConnection)
                {
                    _currentOldConnection = value;                    
                    OnPropertyChanged();
                }
            }
        }

        public PingState CurrentPingState
        {
            get { return _pingState; }
            set
            {
                if (value != _pingState)
                {
                    _pingState = value;
                    OnPropertyChanged();
                }
            }
        }

        public OldConnectionsViewModel OldConnections
        {
            get { return _oldConnections; }
            set
            {
                if (value != _oldConnections)
                {
                    _oldConnections = value;
                    OnPropertyChanged();
                }
            }
        }

        public ConnectionsViewModel Connections
        {
            get { return _connections; }
            set
            {
                if (value != _connections)
                {
                    _connections = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Replies
        {
            get { return _replies; }
            set
            {
                if (value != _replies)
                {
                    _replies = value;
                    OnPropertyChanged();
                }
            }
        }        

        public bool PingExecuted
        {
            get { return _pingExecuted; }
            set
            {
                if (value != _pingExecuted)
                {
                    _pingExecuted = value;
                    if (!_pingExecuted)
                    {                        
                        _pingThread.PingReplyReceived -= _pingReplyReceived;
                        _pingThread.PingStopped -= _pingStopped;
                        _pingThread.StopPing();
                        _pingThread = null;                        
                        CurrentPingState = PingState.PingStopped;
                    }
                    else
                    {
                        ConnectionViewModel connection;
                        if (CurrentTab == 0)
                        {
                            if (CurrentOldConnection == null)
                            {
                                connection = new ConnectionViewModel(CurrentOldConnectionText);
                                _oldConnections.Add(connection);
                            }
                            else
                            {
                                connection = CurrentOldConnection;
                                _oldConnections.MoveToTop(connection);
                            }
                            CurrentOldConnection = connection;
                        }
                        else
                        {
                            connection = CurrentConnection;
                        }                        
                        _listReplies.Clear();
                        _pingThread = new PingThread();
                        _pingThread.PingReplyReceived += _pingReplyReceived;
                        _pingThread.PingStopped += _pingStopped;
                        _pingThread.StartPing(connection.Connection);
                    }
                    OnPropertyChanged();
                }
            }
        }                

        public SettingsViewModel Settings
        {
            get { return _settingsViewModel; }
            set
            {
                if(value != _settingsViewModel)
                {
                    _settingsViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        private Command _executePing = null;
        private Command _stopPing = null;
        private Command _windowLoaded = null;
        private Command _windowClose = null;
        private Command _windowClosed = null;
        private Command _addConnection = null;
        private Command _changeConnection = null;
        private Command _removeConnection = null;
        private Command _test = null;        

        public Command StopPing
        {
            get
            {
                if (_stopPing == null)
                    _stopPing = new Command(() =>
                    {
                        PingExecuted = false;
                    }
                    );
                return _stopPing;
            }
        }

        public Command ExecutePing
        {
            get 
            {
                if (_executePing == null)
                    _executePing = new Command(() =>
                    {
                        PingExecuted = true;
                    });
                return _executePing; 
            }            
        }

        public Command WindowLoaded
        {
            get
            {
                if (_windowLoaded == null)
                    _windowLoaded = new Command(() =>
                    {
                        if (_oldConnections.Count == 0)
                            CurrentOldConnectionText = "";
                        else
                            CurrentOldConnection = _oldConnections[0];
                        if (_connections.Count == 0)
                        {
                            ChangeConnection.CanExecute = false;
                            RemoveConnection.CanExecute = false;
                        }
                        else
                        {
                            ChangeConnection.CanExecute = true;
                            RemoveConnection.CanExecute = true;
                            CurrentConnection = _connections[0];
                        }
                    });
                return _windowLoaded;
            }            
        }

        public Command WindowClose
        {
            get 
            {
                if (_windowClose == null)
                    _windowClose = new Command(() => OnClosingRequest());
                return _windowClose; 
            }            
        }

        public Command WindowClosed
        {
            get
            {
                if (_windowClosed == null)
                    _windowClosed = new Command(() =>
                    {
                        if (PingExecuted)
                        {
                            PingExecuted = false;
                        }
                    });
                return _windowClosed;
            }            
        }

        public Command AddConnection
        {
            get
            {
                if (_addConnection == null)
                    _addConnection = new Command(() =>
                    {
                        DialogAddChangeConnectionViewModel dialogModel = new DialogAddChangeConnectionViewModel();
                        bool? result = new DialogAddChangeConnectionService().ShowDialog(dialogModel);
                        if (result.HasValue && result.Value)
                        {
                            _connections.Add(dialogModel.Connection);                            
                            CurrentConnection = dialogModel.Connection;
                            ChangeConnection.CanExecute = true;
                            RemoveConnection.CanExecute = true;
                        }
                    });
                return _addConnection;
            }
        }

        public Command ChangeConnection
        {
            get
            {
                if (_changeConnection == null)
                    _changeConnection = new Command(() =>
                    {                                                
                        DialogAddChangeConnectionViewModel dialogModel = new DialogAddChangeConnectionViewModel(new ConnectionViewModel(CurrentConnection.Name, CurrentConnection.Host));
                        bool? result = new DialogAddChangeConnectionService().ShowDialog(dialogModel);
                        if (result.HasValue && result.Value)
                        {
                            ConnectionViewModel connection = CurrentConnection;
                            connection.Name = dialogModel.Connection.Name;
                            connection.Host = dialogModel.Connection.Host;                            
                            CurrentConnection = connection;

                        }
                    });
                return _changeConnection;
            }
        }

        public Command RemoveConnection
        {
            get
            {
                if (_removeConnection == null)
                    _removeConnection = new Command(() =>
                    {
                        int index = Connections.IndexOf(CurrentConnection);
                        Connections.Remove(CurrentConnection);
                        if (index < Connections.Count)
                            CurrentConnection = Connections[index];
                        else
                        {
                            if (Connections.Count > 0)
                                CurrentConnection = Connections[Connections.Count - 1];
                            else
                            {
                                ChangeConnection.CanExecute = false;
                                RemoveConnection.CanExecute = false;
                            }
                        }
                    });
                return _removeConnection;
            }
        }        

        public Command Test
        {
            get
            {
                if (_test == null)
                    _test = new Command(() =>
                    {
                        OldConnectionsRepository temp = new OldConnectionsRepository();
                        Debug.WriteLine(temp.GetAll().Count());
                    });
                return _test;
            }
        }

        #endregion

        #region Events

        public event EventHandler ClosingRequest = delegate { };
        private void OnClosingRequest()
        {
            ClosingRequest(this, EventArgs.Empty);
        }

        #endregion

        #region IDataErrorInfo Members

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
                if(columnName== "CurrentOldConnectionText")
                {
                    if(Uri.CheckHostName(CurrentOldConnectionText)==UriHostNameType.Unknown)
                    {
                        result = Localization.Localization.AddressIsInvalid;
                        CurrentOldConnectionIsInvalid = true;
                    }
                    else
                    {
                        CurrentOldConnectionIsInvalid = false;
                    }
                }
                return result;
            }
        }

        #endregion
    }

    enum PingState
    {
        GoodSignal,
        BadSignal,
        NoSignal,
        PingStopped
    }
}
