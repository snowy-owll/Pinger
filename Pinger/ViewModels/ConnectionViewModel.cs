using Pinger.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger.ViewModels
{
    class ConnectionViewModel : ObservableObject
    {        
        public ConnectionViewModel(Connection connection)
        {
            _connection = connection;
        }

        public ConnectionViewModel(string name, string host) : this(new Connection(name, host)) { }

        public ConnectionViewModel(string host) : this(new Connection(host)) { }

        public ConnectionViewModel() : this(new Connection()) { }

        private Connection _connection;

        public int ID
        {
            get { return _connection.ID; }
            set
            {
                if (value != _connection.ID)
                {
                    _connection.ID = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return _connection.Name; }
            set 
            {
                if (value != _connection.Name)
                {
                    _connection.Name = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string Host
        {
            get { return _connection.Host; }
            set
            {
                if (value != _connection.Host)
                {
                    _connection.Host = value; 
                    OnPropertyChanged();
                }
            }
        }

        public Connection Connection
        {
            get { return _connection; }
        }
    }
}
