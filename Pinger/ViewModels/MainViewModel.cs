using Infralution.Localization.Wpf;
using Pinger.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Resources;
using System.Windows.Threading;
using System.Diagnostics;
using Pinger.Models;
using System.Linq;

namespace Pinger.ViewModels
{
    class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _oldConnections = new OldConnectionsViewModel(_settings);
            _connections = new ConnectionsViewModel(_settings);
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
            _soundPing = _settings.SoundPing;
            _pingExecuted = false;
            CurrentPingState = PingState.PingStopped;

            _supportedCultures = new ObservableCollection<CultureItem>();
            SupportedCultures.Add(new CultureItem(CultureInfo.InstalledUICulture, true));
            ResourceManager rm = new ResourceManager(typeof(Localization.Localization));            
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    ResourceSet rs = rm.GetResourceSet(culture, true, false);                    
                    if (rs == null || culture.Equals(CultureInfo.InvariantCulture)) continue;
                    SupportedCultures.Add(new CultureItem(culture));
                }
                catch (CultureNotFoundException)
                {
                    Debug.WriteLine(culture + " is not available on the machine or is an invalid culture identifier.");
                }
            }
            CurrentCulture = SupportedCultures.Where(c => c.CultureInfo.Equals(_settings.Language.CultureInfo)).First();
            Debug.WriteLine("");
        }

        private Settings _settings = new Settings();
        private ObservableCollection<int> _listReplies = new ObservableCollection<int>();
        private PingThread _pingThread = null;
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private DialogAddChangeConnectionService _dialogAddChangeConnectionService = new DialogAddChangeConnectionService();
        private DialogAddChangeConnectionViewModel _dialogAddChangeConnectionModel = new DialogAddChangeConnectionViewModel();

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
        private ConnectionViewModel _currentOldConnection;
        private PingState _pingState = PingState.PingStopped;
        private OldConnectionsViewModel _oldConnections;
        private ConnectionsViewModel _connections;
        private string _replies;
        private SoundPing _soundPing;
        private bool _pingExecuted;
        private bool _canChangeRemoveConnection;
        private bool _canPingStart;
        private ObservableCollection<CultureItem> _supportedCultures;
        private CultureItem _currentCulture;

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
                        if (CurrentOldConnectionText == "")
                            CanPingStart = false;
                        else
                            CanPingStart = true;
                    }
                    else
                    {
                        if (CurrentConnection == null)
                            CanPingStart = false;
                        else
                            CanPingStart = true;
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
                        CanPingStart = false;
                    else if (_currentConnection != null && CurrentTab == 1)
                        CanPingStart = true;
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
                    if (_currentOldConnectionText == "" && CurrentTab==0)
                        CanPingStart = false;
                    else if (_currentOldConnectionText != "" && CurrentTab == 0)
                        CanPingStart = true;
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

        public SoundPing SoundPing
        {
            get { return _soundPing; }
            set
            {
                if (value != _soundPing)
                {
                    _soundPing = value;
                    _settings.SoundPing = value;
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
                                _oldConnections.List.Insert(0, connection);
                            }
                            else
                            {
                                connection = CurrentOldConnection;
                                _oldConnections.List.Move(CurrentOldConnectionIndex, 0);
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

        public bool CanChangeRemoveConnection
        {
            get { return _canChangeRemoveConnection; }
            set
            {
                if (value != _canChangeRemoveConnection)
                {
                    _canChangeRemoveConnection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanPingStart
        {
            get { return _canPingStart; }
            set
            {
                if (value != _canPingStart)
                {
                    _canPingStart = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<CultureItem> SupportedCultures
        {
            get { return _supportedCultures; }
            set
            {
                if (value != _supportedCultures)
                {
                    _supportedCultures = value;
                    OnPropertyChanged();
                }
            }
        }

        public CultureItem CurrentCulture
        {
            get { return _currentCulture; }
            set
            {
                if (value != _currentCulture)
                {
                    _currentCulture = value;
                    _settings.Language = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        private Command _executeStopPing = null;
        private Command _windowLoaded = null;
        private Command _windowClose = null;
        private Command _windowClosed = null;
        private Command _addConnection = null;
        private Command _changeConnection = null;
        private Command _removeConnection = null;
        private Command _test = null;

        public Command ExecuteStopPing
        {
            get 
            {
                if (_executeStopPing == null)
                    _executeStopPing = new Command(() =>
                    {
                        PingExecuted = !PingExecuted;
                    });
                return _executeStopPing; 
            }            
        }

        public Command WindowLoaded
        {
            get
            {
                if (_windowLoaded == null)
                    _windowLoaded = new Command(() =>
                    {
                        if (_oldConnections.List.Count == 0)
                            CurrentOldConnectionText = "";
                        else
                            CurrentOldConnection = _oldConnections.List[0];
                        if (_connections.List.Count == 0)
                            CanChangeRemoveConnection = false;
                        else
                        {
                            CanChangeRemoveConnection = true;
                            CurrentConnection = _connections.List[0];
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
                        _dialogAddChangeConnectionModel.State = DialogAddChangeConnectionState.Add;
                        _dialogAddChangeConnectionModel.Connection = new ConnectionViewModel();
                        bool? result = _dialogAddChangeConnectionService.ShowDialog(_dialogAddChangeConnectionModel);
                        if (result.HasValue && result.Value)
                        {
                            _connections.List.Add(_dialogAddChangeConnectionModel.Connection);
                            _connections.Sort();
                            CurrentConnection = _dialogAddChangeConnectionModel.Connection;
                            CanChangeRemoveConnection = true;
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
                        _dialogAddChangeConnectionModel.State = DialogAddChangeConnectionState.Change;                        
                        _dialogAddChangeConnectionModel.Connection = new ConnectionViewModel(CurrentConnection.Name, CurrentConnection.Host);;
                        bool? result = _dialogAddChangeConnectionService.ShowDialog(_dialogAddChangeConnectionModel);
                        if (result.HasValue && result.Value)
                        {
                            ConnectionViewModel connection = CurrentConnection;
                            connection.Name = _dialogAddChangeConnectionModel.Connection.Name;
                            connection.Host = _dialogAddChangeConnectionModel.Connection.Host;
                            Connections.Sort();
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
                        int index = Connections.List.IndexOf(CurrentConnection);
                        Connections.List.Remove(CurrentConnection);
                        if (index < Connections.List.Count)
                            CurrentConnection = Connections.List[index];
                        else
                            if (Connections.List.Count > 0)
                                CurrentConnection = Connections.List[Connections.List.Count - 1];
                            else
                                CanChangeRemoveConnection = false;
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
                        //CurrentPingState = PingState.GoodSignal;

                        Debug.WriteLine(CultureInfo.InstalledUICulture);
                        CultureManager.UICulture = new CultureInfo("en");
                        Debug.WriteLine(CultureInfo.InstalledUICulture);                        

                        /*ResourceManager rm = new ResourceManager(typeof(Localization.Localization));
                        List<string> temp = new List<string>();
                        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                        foreach (CultureInfo culture in cultures)
                        {
                            try
                            {
                                ResourceSet rs = rm.GetResourceSet(culture, true, false);
                                // or ResourceSet rs = rm.GetResourceSet(new CultureInfo(culture.TwoLetterISOLanguageName), true, false);
                                if (rs == null) continue;
                                string isSupported = (rs == null) ? " is not supported" : " is supported";
                                temp.Add(culture + isSupported);                                
                            }
                            catch (CultureNotFoundException exc)
                            {
                                //temp.Add(culture + " is not available on the machine or is an invalid culture identifier.");
                            }
                        }
                        Debug.WriteLine(temp.Count);  */                      
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
    }

    enum PingState
    {
        GoodSignal,
        BadSignal,
        NoSignal,
        PingStopped
    }
}
