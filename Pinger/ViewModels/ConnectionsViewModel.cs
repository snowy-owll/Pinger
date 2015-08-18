using Pinger.Collections;
using Pinger.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Pinger.ViewModels
{
    class ConnectionsViewModel : ConnectionsCollection
    {
        public ConnectionsViewModel(Settings settings) : base()
        {
            _settings = settings;
            foreach (Connection connection in _settings.GetConnections())
            {
                ConnectionViewModel model = new ConnectionViewModel(connection);
                this.Add(model);
                model.PropertyChanged += _itemChanged;
            }
            this.CollectionChanged += (s, e) =>
                {
                    ConnectionViewModel connection;
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            connection = (ConnectionViewModel)e.NewItems[0];
                            connection.PropertyChanged += _itemChanged;
                            _settings.AddConnection(connection.Connection);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            connection = (ConnectionViewModel)e.OldItems[0];
                            connection.PropertyChanged -= _itemChanged;
                            _settings.RemoveConnection(connection.Connection);
                            break;
                    }
                };
        }

        private Settings _settings;

        private void _itemChanged(object sender, PropertyChangedEventArgs e)
        {
            _settings.ChangeConnection((sender as ConnectionViewModel).Connection);
        }
    }
}
