using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinger.Models
{
    class Connection
    {
        public Connection(string name, string host)
        {
            _name = name;
            _host = host;
        }

        public Connection(string host) : this("", host) { }

        public Connection(int id, string name, string host) : this(name, host)
        {
            _id = id;            
        }

        public Connection(int id, string host) : this(id, "", host) { }

        public Connection() : this("", "") { }

        private int _id;
        private string _name;
        private string _host;

        public int ID {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }        

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }
    }
}
