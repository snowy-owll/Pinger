﻿using Pinger.Models;
using System.Linq;

namespace Pinger.Interfaces
{
    interface IRepository<T> where T: Entity
    {
        IQueryable<T> GetAll();
        bool Save(T entity);
        bool Delete(int id);
        bool Delete(T entity);
    }
}
