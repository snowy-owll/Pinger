using Pinger.Interfaces;
using Pinger.Models;
using System;
using System.Data.Linq;
using System.Data.SqlServerCe;
using System.Linq;
using System.Linq.Expressions;

namespace Pinger.DB
{
    public abstract class RepositoryBase<T, DbT> : IRepository<T>
    where T : Entity where DbT : class, IDbEntity, new()
    {
        SqlCeConnection _connection = new SqlCeConnection("Data Source=db.sdf");
        protected readonly dbContext context;

        public RepositoryBase()
        {
            context = new dbContext(_connection);
        }

        public IQueryable<T> GetAll()
        {
            return GetTable().Select(GetConverter());
        }

        public bool Save(T entity)
        {
            DbT dbEntity;

            if (entity.IsNew())
            {
                dbEntity = new DbT();
            }
            else
            {
                dbEntity = GetTable().Where(x => x.Id.Equals(entity.Id)).SingleOrDefault();
                if (dbEntity == null)
                {
                    return false;
                }
            }

            UpdateEntry(dbEntity, entity);

            if (entity.IsNew())
            {
                GetTable().InsertOnSubmit(dbEntity);
            }

            context.SubmitChanges();

            entity.Id = dbEntity.Id;
            return true;
        }

        public bool Delete(int id)
        {
            var dbEntity = GetTable().Where(x => x.Id.Equals(id)).SingleOrDefault();

            if (dbEntity == null)
            {
                return false;
            }

            GetTable().DeleteOnSubmit(dbEntity);

            context.SubmitChanges();
            return true;
        }

        public bool Delete(T entity)
        {
            return Delete(entity.Id);
        }

        protected abstract Table<DbT> GetTable();
        protected abstract Expression<Func<DbT, T>> GetConverter();
        protected abstract void UpdateEntry(DbT dbEntity, T entity);
    }
}
