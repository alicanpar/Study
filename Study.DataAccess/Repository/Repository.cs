﻿using Microsoft.EntityFrameworkCore;
using Study.DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace Study.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties=null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperies = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (includeProperies != null)
            {
                foreach (var includeProp in includeProperies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }
        public void Remove(T entitiy)
        {
            dbSet.Remove(entitiy);
        }
    }
}
