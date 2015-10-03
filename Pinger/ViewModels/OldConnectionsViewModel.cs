using Pinger.Collections;
using Pinger.Models;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Pinger.ViewModels
{
    class OldConnectionsViewModel: OldConnectionsCollection
    {
        const int MAX_OLD_CONNECTIONS = 10;

        public OldConnectionsViewModel(Settings settings) : base(settings.GetOldConnections().ConvertAll(x=>new ConnectionViewModel(x)))
        {
            _settings = settings;            
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
                        movedConnection.Connection.Id = -1;
                        _settings.AddOldConnection(movedConnection.Connection);
                    }
                };
        }

        private Settings _settings;        
    }
}
