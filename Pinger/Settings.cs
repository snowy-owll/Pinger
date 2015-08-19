using Infralution.Localization.Wpf;
using Pinger.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Linq;
using Pinger.DB;
using Pinger.Interfaces;

namespace Pinger
{
    class Settings
    {        
        public Settings()
        {
            SqlCeConnection _connection = new SqlCeConnection("Data Source=db.sdf");
            
            _maxRepliesCount = Int32.Parse(_settingsRepository.GetAll().Where(r => r.Name.Equals("MaxRepliesCount")).First().Value);
            _soundPing = (SoundPing)Enum.Parse(typeof(SoundPing), _settingsRepository.GetAll().Where(r => r.Name == "SoundPing").First().Value);
            string language = _settingsRepository.GetAll().Where(r => r.Name == "Language").First().Value;
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
            _oldConnectionsRepository.Save(connection);
        }

        public void RemoveOldConnection(Connection connection)
        {
            _oldConnectionsRepository.Delete(connection);
        }

        public List<Connection> GetOldConnections()
        {            
            return _oldConnectionsRepository.GetAll().OrderByDescending(x => x.Id).ToList();
        }        

        public List<Connection> GetConnections()
        {            
            return _connectionsRepository.GetAll().ToList();
        }

        public void AddConnection(Connection connection)
        {
            _connectionsRepository.Save(connection);
        }

        public void RemoveConnection(Connection connection)
        {
            _connectionsRepository.Delete(connection);
        }

        public void ChangeConnection(Connection connection)
        {
            _connectionsRepository.Save(connection);
        }

        private IRepository<Setting> _settingsRepository = new SettingsRepository();
        private IRepository<Connection> _connectionsRepository = new ConnectionsRepository();
        private IRepository<Connection> _oldConnectionsRepository = new OldConnectionsRepository();

        private SoundPing _soundPing;
        public SoundPing SoundPing { 
            get
            {
                return _soundPing;
            }
            set 
            {
                _soundPing = value;
                Setting s = _settingsRepository.GetAll().Where(r => r.Name.Equals("SoundPing")).First();
                s.Value = value.ToString();
                _settingsRepository.Save(s);
            }
        }

        private int _maxRepliesCount;
        public int MaxRepliesCount {
            get { return _maxRepliesCount; }
            set 
            {
                _maxRepliesCount = value;
                Setting s = _settingsRepository.GetAll().Where(r => r.Name.Equals("MaxRepliesCount")).First();
                s.Value = value.ToString();
                _settingsRepository.Save(s);
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
                    Setting s = _settingsRepository.GetAll().Where(r => r.Name.Equals("Language")).First();                    
                    if (value.System)
                        s.Value = "system";
                    else
                        s.Value = value.CultureInfo.Name;
                    _settingsRepository.Save(s);
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
