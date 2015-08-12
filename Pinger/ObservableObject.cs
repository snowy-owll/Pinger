using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger
{
    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {            
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
