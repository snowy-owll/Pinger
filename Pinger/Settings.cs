using Infralution.Localization.Wpf;
using Pinger.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Pinger
{
    class Settings
    {
        dbContext _context;
        
        public Settings()
        {
            SqlCeConnection _connection = new SqlCeConnection("Data Source=db.sdf");
            _context = new dbContext(_connection);
            _context.Log = new TextWriterDebug();
            if(!_context.DatabaseExists())
            {
                _context.CreateDatabase();
                SettingsTable setting = new SettingsTable()
                {
                    Name = "SoundPing",
                    Value = "SignalIsGood"
                };
                _context.SettingsTable.InsertOnSubmit(setting);
                setting = new SettingsTable()
                {
                    Name = "MaxRepliesCount",
                    Value = "15"
                };
                _context.SettingsTable.InsertOnSubmit(setting);
                setting = new SettingsTable()
                {
                    Name = "Language",
                    Value = "system"
                };
                _context.SettingsTable.InsertOnSubmit(setting);
                _context.SubmitChanges();
            }            
            _maxRepliesCount = Int32.Parse(_context.SettingsTable.Where(r => r.Name.Equals("MaxRepliesCount")).First().Value);
            _soundPing = (SoundPing)Enum.Parse(typeof(SoundPing), _context.SettingsTable.Where(r => r.Name == "SoundPing").First().Value);
            string language = _context.SettingsTable.Where(r => r.Name == "Language").First().Value;
            if(language=="system")
            {
                _language = new CultureItem(CultureInfo.InstalledUICulture, true);                
            }
            else
            {
                _language = new CultureItem(new CultureInfo(language));
            }
            CultureManager.UICulture = _language.CultureInfo;
        }

        public void AddOldConnection(Connection connection)
        {
            OldConnectionsTable newConnection = new OldConnectionsTable() { Host = connection.Host };
            _context.OldConnectionsTable.InsertOnSubmit(newConnection);
            _context.SubmitChanges();
            connection.ID = newConnection.Id;
        }

        public void RemoveOldConnection(Connection connection)
        {
            OldConnectionsTable item = _context.OldConnectionsTable.Where(row => row.Id == connection.ID).First();
            _context.OldConnectionsTable.DeleteOnSubmit(item);            
            _context.SubmitChanges();
        }

        public List<Connection> GetOldConnections()
        {
            List<OldConnectionsTable> list = _context.OldConnectionsTable.ToList<OldConnectionsTable>();
            List<Connection> oldConnections = new List<Connection>();
            foreach (OldConnectionsTable item in list)
            {
                oldConnections.Add(new Connection(item.Id, item.Host));
            }
            return oldConnections;
        }        

        public List<Connection> GetConnections()
        {
            List<ConnectionsTable> list = _context.ConnectionsTable.ToList<ConnectionsTable>();
            List<Connection> connections = new List<Connection>();
            foreach (ConnectionsTable item in list)
            {
                connections.Add(new Connection(item.Id,item.Name, item.Host));
            }
            return connections;
        }

        public void AddConnection(Connection connection)
        {
            ConnectionsTable newConnection = new ConnectionsTable()
            {
                Name = connection.Name,
                Host = connection.Host
            };
            _context.ConnectionsTable.InsertOnSubmit(newConnection);
            _context.SubmitChanges();
            connection.ID = newConnection.Id;
        }

        public void RemoveConnection(Connection connection)
        {
            ConnectionsTable item = _context.ConnectionsTable.Where(row => row.Id == connection.ID).First();
            _context.ConnectionsTable.DeleteOnSubmit(item);
            _context.SubmitChanges();
        }

        public void ChangeConnection(Connection connection)
        {
            ConnectionsTable item = _context.ConnectionsTable.Where(row => row.Id == connection.ID).First();
            item.Name = connection.Name;
            item.Host = connection.Host;
            _context.SubmitChanges();
        }

        private SoundPing _soundPing;
        public SoundPing SoundPing { 
            get
            {
                return _soundPing;
            }
            set 
            {
                SettingsTable s = _context.SettingsTable.Where(r => r.Name.Equals("SoundPing")).First();
                s.Value = value.ToString();
                _soundPing = value;
                _context.SubmitChanges();
            }
        }

        private int _maxRepliesCount;
        public int MaxRepliesCount {
            get { return _maxRepliesCount; }
            set 
            {
                SettingsTable s = _context.SettingsTable.Where(r => r.Name == "MaxRepliesCount").First();
                s.Value = value.ToString();
                _maxRepliesCount = value;
                _context.SubmitChanges();
            }
        }

        private CultureItem _language;
        public CultureItem Language
        {
            get 
            { 
                return _language; 
            }
            set 
            {
                if (_language != value) 
                {
                    _language = value;
                    SettingsTable s = _context.SettingsTable.Where(r => r.Name == "Language").First();
                    if (value.System)
                        s.Value = "system";
                    else
                        s.Value = value.CultureInfo.Name;
                    _context.SubmitChanges();
                    CultureManager.UICulture = value.CultureInfo;
                }                
            }
        }

    }

    enum SoundPing
    {
        Never,
        SignalIsGood,
        NoSignal
    }
}
