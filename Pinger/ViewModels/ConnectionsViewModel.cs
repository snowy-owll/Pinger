using Pinger.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Pinger.ViewModels
{
    class ConnectionsViewModel
    {        
        public ConnectionsViewModel(Settings settings)
        {            
            _settings = settings;
            _connections = new ObservableCollection<ConnectionViewModel>();            
            foreach(Connection connection in _settings.GetConnections())
            {
                _connections.Add(new ConnectionViewModel(connection));
                _connections[_connections.Count - 1].PropertyChanged += _itemChanged;
            }
            Sort();
            _connections.CollectionChanged += (s, e) =>
                {
                    if(e.Action==NotifyCollectionChangedAction.Add)
                    {
                        ConnectionViewModel connection = (ConnectionViewModel)e.NewItems[0];
                        connection.PropertyChanged += _itemChanged;
                        _settings.AddConnection(connection.Connection);                        
                        return;
                    }
                    if(e.Action==NotifyCollectionChangedAction.Remove)
                    {
                        ConnectionViewModel connection = (ConnectionViewModel)e.OldItems[0];
                        connection.PropertyChanged -= _itemChanged;
                        _settings.RemoveConnection(connection.Connection);
                        return;
                    }                    
                };
        }

        private Settings _settings;
        private ObservableCollection<ConnectionViewModel> _connections;

        public ObservableCollection<ConnectionViewModel> List 
        {
            get { return _connections; }            
        }

        private void _itemChanged(object sender, PropertyChangedEventArgs e)
        {
            _settings.ChangeConnection((sender as ConnectionViewModel).Connection);            
        }

        public void Sort()
        {
            if (_connections.Count < 2)
                return;
            Action<int, int> sort = null;
            sort = (low, high) =>
                {
                    int i = low;
                    int j = high;
                    ConnectionViewModel m = _connections[(i + j) / 2];
                    do
                    {
                        while (StringLogicalComparer.Compare(_connections[i].Name, m.Name) < 0) i++;
                        while (StringLogicalComparer.Compare(_connections[j].Name, m.Name) > 0) j--;
                        if (i <= j)
                        {
                            ConnectionViewModel temp = _connections[i];
                            _connections[i] = _connections[j];
                            _connections[j] = temp;
                            i++; j--;
                        }
                    } while (i <= j);
                    if (low < j) sort(low, j);
                    if (i < high) sort(i, high);
                };
            sort(0, _connections.Count - 1);
        }        
    }
}
