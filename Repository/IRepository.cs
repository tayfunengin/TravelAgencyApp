using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        string Insert(T entity);
        string Update(T entity);
        string Delete(int id);
        void SaveChanges();
        IEnumerable<T> GetDefault(Expression<Func<T,bool>> exp);
        bool Any(Expression<Func<T, bool>> exp);
    }
}
