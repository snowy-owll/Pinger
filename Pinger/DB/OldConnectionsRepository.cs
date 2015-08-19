using Pinger.Models;
using System;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Pinger.DB
{
    class OldConnectionsRepository : RepositoryBase<Connection, OldConnectionsTable>
    {
        protected override Expression<Func<OldConnectionsTable, Connection>> GetConverter()
        {
            return c => new Connection
            {
                Id = c.Id,
                Host = c.Host
            };
        }

        protected override Table<OldConnectionsTable> GetTable()
        {
            return context.OldConnectionsTable;
        }

        protected override void UpdateEntry(OldConnectionsTable dbOldConnection, Connection oldConnection)
        {
            dbOldConnection.Host = oldConnection.Host;
        }
    }
}
