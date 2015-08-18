using Pinger.Collections;
using Pinger.Models;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Pinger.ViewModels
{
    class OldConnectionsViewModel: OldConnectionsCollection
    {
        const int MAX_OLD_CONNECTIONS = 15;

        public OldConnectionsViewModel(Settings settings) : base()
        {
            _settings = settings;
            List<Connection> list = _settings.GetOldConnections();
            list.Sort(delegate (Connection p1, Connection p2)
            {
                return p1.ID.CompareTo(p2.ID);
            });            
            foreach (Connection connection in list)
                Add(new ConnectionViewModel(connection));
            CollectionChanged += (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        if (e.NewStartingIndex != 0) return;
                        ConnectionViewModel newConnection = (ConnectionViewModel)e.NewItems[0];
                        _settings.AddOldConnection(newConnection.Connection);
                        if (Count > MAX_OLD_CONNECTIONS)
                        {
                            Remove(this[Count - 1]);
                        }
                        return;
                    }
                    if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        ConnectionViewModel removedConnection = (ConnectionViewModel)e.OldItems[0];
                        _settings.RemoveOldConnection(removedConnection.Connection);
                        return;
                    }
                    if (e.Action == NotifyCollectionChangedAction.Move)
                    {
                        if (e.NewStartingIndex != 0) return;
                        ConnectionViewModel movedConnection = (ConnectionViewModel)e.NewItems[0];
                        _settings.RemoveOldConnection(movedConnection.Connection);
                        _settings.AddOldConnection(movedConnection.Connection);
                    }
                };
        }

        private Settings _settings;        
    }
}
