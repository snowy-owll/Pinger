namespace Pinger.Models
{
    class Connection : Entity
    {
        public Connection(string name, string host)
        {
            Name = name;
            Host = host;
        }

        public Connection(string host) : this("", host) { }

        public Connection(int id, string name, string host) : this(name, host)
        {
            Id = id;
        }

        public Connection(int id, string host) : this(id, "", host) { }

        public Connection() : this("", "") { }        

        public string Name { get; set; }

        public string Host { get; set; }
    }
}
