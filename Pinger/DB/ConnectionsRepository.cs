using System;
using System.Data.Linq;
using System.Linq.Expressions;
using Pinger.Models;

namespace Pinger.DB
{
    class ConnectionsRepository : RepositoryBase<Connection, ConnectionsTable>
    {
        protected override Expression<Func<ConnectionsTable, Connection>> GetConverter()
        {
            return c => new Connection
            {
                Id = c.Id,
                Name = c.Name,
                Host = c.Host
            };
        }

        protected override Table<ConnectionsTable> GetTable()
        {
            return context.ConnectionsTable;
        }

        protected override void UpdateEntry(ConnectionsTable dbConnection, Connection connection)
        {
            dbConnection.Name = connection.Name;
            dbConnection.Host = connection.Host;
        }
    }
}
