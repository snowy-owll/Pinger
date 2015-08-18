using Pinger.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pinger.Collections
{
    class OldConnectionsCollection : ObservableCollection<ConnectionViewModel>
    {
        public OldConnectionsCollection()
        : base()
        {
        }        

        protected override void InsertItem(int index, ConnectionViewModel item)
        {
            this.CheckReentrancy();
            int i = 0;
            base.InsertItem(i, item);
        }

        public void MoveToTop(ConnectionViewModel item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == item)
                {
                    MoveItem(i, 0);
                    return;
                }
            }
        }
    }
}
