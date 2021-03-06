﻿#pragma warning disable 1591

namespace Pinger.DB
{
    using System.Data.Linq.Mapping;
    using System.ComponentModel;
    using System;
    using Interfaces;

    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "db.sdf")]
    public partial class dbContext : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Определения метода расширяемости
        partial void OnCreated();
        partial void InsertConnectionsTable(ConnectionsTable instance);
        partial void UpdateConnectionsTable(ConnectionsTable instance);
        partial void DeleteConnectionsTable(ConnectionsTable instance);
        partial void InsertOldConnectionsTable(OldConnectionsTable instance);
        partial void UpdateOldConnectionsTable(OldConnectionsTable instance);
        partial void DeleteOldConnectionsTable(OldConnectionsTable instance);
        partial void InsertSettingsTable(SettingsTable instance);
        partial void UpdateSettingsTable(SettingsTable instance);
        partial void DeleteSettingsTable(SettingsTable instance);
        #endregion

        public dbContext(string connection) :
                base(connection, mappingSource)
        {
            _verifyExistenceOfDb();
            Log = new TextWriterDebug();
            OnCreated();
        }

        public dbContext(System.Data.IDbConnection connection) :
                base(connection, mappingSource)
        {
            _verifyExistenceOfDb();
            Log = new TextWriterDebug();
            OnCreated();
        }

        public dbContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
                base(connection, mappingSource)
        {
            _verifyExistenceOfDb();
            Log = new TextWriterDebug();
            OnCreated();
        }

        public dbContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
                base(connection, mappingSource)
        {
            _verifyExistenceOfDb();
            Log = new TextWriterDebug();
            OnCreated();
        }

        public System.Data.Linq.Table<ConnectionsTable> ConnectionsTable
        {
            get
            {
                return this.GetTable<ConnectionsTable>();
            }
        }

        public System.Data.Linq.Table<OldConnectionsTable> OldConnectionsTable
        {
            get
            {
                return this.GetTable<OldConnectionsTable>();
            }
        }

        public System.Data.Linq.Table<SettingsTable> SettingsTable
        {
            get
            {
                return this.GetTable<SettingsTable>();
            }
        }

        private  void _verifyExistenceOfDb()
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
                SettingsTable setting = new SettingsTable()
                {
                    Name = "SoundPing",
                    Value = "SignalIsGood"
                };
                SettingsTable.InsertOnSubmit(setting);
                setting = new SettingsTable()
                {
                    Name = "MaxRepliesCount",
                    Value = "15"
                };
                SettingsTable.InsertOnSubmit(setting);
                setting = new SettingsTable()
                {
                    Name = "Language",
                    Value = "system"
                };
                SettingsTable.InsertOnSubmit(setting);
                SubmitChanges();
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class ConnectionsTable : IDbEntity, INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private string _Name;

        private string _Host;

        #region Определения метода расширяемости
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnHostChanging(string value);
        partial void OnHostChanged();
        #endregion

        public ConnectionsTable()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "name", Storage = "_Name", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "host", Storage = "_Host", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Host
        {
            get
            {
                return this._Host;
            }
            set
            {
                if ((this._Host != value))
                {
                    this.OnHostChanging(value);
                    this.SendPropertyChanging();
                    this._Host = value;
                    this.SendPropertyChanged("Host");
                    this.OnHostChanged();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class OldConnectionsTable : IDbEntity, INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private string _Host;

        #region Определения метода расширяемости
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnHostChanging(string value);
        partial void OnHostChanged();
        #endregion

        public OldConnectionsTable()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "host", Storage = "_Host", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Host
        {
            get
            {
                return this._Host;
            }
            set
            {
                if ((this._Host != value))
                {
                    this.OnHostChanging(value);
                    this.SendPropertyChanging();
                    this._Host = value;
                    this.SendPropertyChanged("Host");
                    this.OnHostChanged();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class SettingsTable : IDbEntity, INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private string _Name;

        private string _Value;

        #region Определения метода расширяемости
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnValueChanging(string value);
        partial void OnValueChanged();
        #endregion

        public SettingsTable()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "name", Storage = "_Name", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "value", Storage = "_Value", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                if ((this._Value != value))
                {
                    this.OnValueChanging(value);
                    this.SendPropertyChanging();
                    this._Value = value;
                    this.SendPropertyChanged("Value");
                    this.OnValueChanged();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
#pragma warning restore 1591
