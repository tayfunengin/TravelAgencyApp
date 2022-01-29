using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string result = string.Empty;
        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return entities.Any(exp);
        }

        public string Delete(int id)
        {
            try
            {
                T deleted = this.GetById(id);
                deleted.Status = Convert.ToBoolean(Status.Passive);
                this.Update(deleted);              
            }
            catch (Exception ex)
            {
                result = NotificationType.exception + ":" + ex;
            }

            return result;
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable<T>();
        }

        public T GetById(int id)
        {
            return entities.Find(id);
        }

        public IEnumerable<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return entities.Where(exp).ToList();
        }

        public string Insert(T entity)
        {            
            if (entity != null)
            {
                try
                {                    
                    entities.Add(entity);
                    context.SaveChanges();
                    result = NotificationType.success + ":" + $"Has been created successfully.";
                }
                catch (Exception ex)
                {
                    result = NotificationType.exception + ":" + ex;
                }
            }
            else
            {
                result = NotificationType.error + ":" + "Entity cannot be empty.";
            }
            return result;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public string Update(T entity)
        {
            if (entity != null)
            {
                try
                {
                    entities.Update(entity);
                    context.SaveChanges();
                    if (entity.Status == Convert.ToBoolean(Status.Passive))
                    {
                        result = NotificationType.success + ":" + $"Entity has been deactivated successfully.";
                    }
                    else
                    {
                        result = NotificationType.success + ":" + $"The update transaction has been completed successfully.";
                    }

                }
                catch (Exception ex)
                {
                    result = NotificationType.exception + ":" + ex;
                }
            }
            else
            {
                result = NotificationType.error + ":" + "Entity cannot be empty.";
            }
            return result;
        }
    }
}
