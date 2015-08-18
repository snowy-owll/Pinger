using Pinger.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Pinger.Collections
{
    /// <summary>
    /// Sorted collection of connections
    /// </summary>
    class ConnectionsCollection : ObservableCollection<ConnectionViewModel>
    {
        public ConnectionsCollection()
        : base()
        {
            this.CollectionChanged += _connectionsCollection_CollectionChanged;
        }

        public ConnectionsCollection(List<ConnectionViewModel> list)
        : base(list)
        {
            foreach (ConnectionViewModel item in this)
            {
                item.PropertyChanged += _item_PropertyChanged;
            }
            this.CollectionChanged += _connectionsCollection_CollectionChanged;
            _sort();
        }

        new public void Add(ConnectionViewModel item)
        {
            base.Add(item);
            _sort();
        }

        private void _connectionsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (ConnectionViewModel item in e.OldItems)
                    {
                        item.PropertyChanged -= _item_PropertyChanged;
                    }                    
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (ConnectionViewModel item in e.NewItems)
                    {
                        item.PropertyChanged += _item_PropertyChanged;
                    }                    
                    break;
            }
        }

        private void _item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                _sort();
            }
        }

        private void _sort()
        {
            if (this.Count < 2)
                return;
            Action<int, int> sort = null;
            sort = (low, high) =>
            {
                int i = low;
                int j = high;
                ConnectionViewModel m = this[(i + j) / 2];
                do
                {
                    while (StringLogicalComparer.Compare(this[i].Name, m.Name) < 0) i++;
                    while (StringLogicalComparer.Compare(this[j].Name, m.Name) > 0) j--;
                    if (i <= j)
                    {
                        ConnectionViewModel temp = this[i];
                        this[i] = this[j];
                        this[j] = temp;
                        i++; j--;
                    }
                } while (i <= j);
                if (low < j) sort(low, j);
                if (i < high) sort(i, high);
            };
            sort(0, this.Count - 1);
        }
    }
}
