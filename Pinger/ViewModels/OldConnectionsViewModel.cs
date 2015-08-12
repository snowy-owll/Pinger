using Pinger.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Pinger.ViewModels
{
    class OldConnectionsViewModel
    {
        public OldConnectionsViewModel(Settings settings)
        {
            _settings = settings;
            _oldConnections = new ObservableCollection<ConnectionViewModel>();
            List<Connection> list = _settings.GetOldConnections();
            list.Sort(delegate(Connection p1, Connection p2){
                return p1.ID.CompareTo(p2.ID);
            });
            list.Reverse();
            foreach (Connection connection in list)
                _oldConnections.Add(new ConnectionViewModel(connection));            
            _oldConnections.CollectionChanged += (s, e) =>
                {
                    if(e.Action==NotifyCollectionChangedAction.Add)
                    {
                        if (e.NewStartingIndex != 0) return;
                        ConnectionViewModel newConnection = (ConnectionViewModel)e.NewItems[0];
                        _settings.AddOldConnection(newConnection.Connection);
                        if (_oldConnections.Count > 15)
                        {
                            _oldConnections.Remove(_oldConnections[_oldConnections.Count-1]);
                        }
                        return;
                    }
                    if(e.Action==NotifyCollectionChangedAction.Remove)
                    {
                        ConnectionViewModel removedConnection = (ConnectionViewModel)e.OldItems[0];
                        _settings.RemoveOldConnection(removedConnection.Connection);
                        return;
                    }
                    if(e.Action==NotifyCollectionChangedAction.Move)
                    {
                        if (e.NewStartingIndex != 0) return;
                        ConnectionViewModel movedConnection = (ConnectionViewModel)e.NewItems[0];
                        _settings.RemoveOldConnection(movedConnection.Connection);
                        _settings.AddOldConnection(movedConnection.Connection);
                    }
                };
        }

        private Settings _settings;
        private ObservableCollection<ConnectionViewModel> _oldConnections;

        public ObservableCollection<ConnectionViewModel> List 
        {
            get { return _oldConnections; }            
        }        
    }
}
