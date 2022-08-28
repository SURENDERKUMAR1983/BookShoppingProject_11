using BookShoppingProject.DataAccess.Data;
using BookShoppingProject.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeproperties = null)
        {
            IQueryable<T> querry = dbset;
            if (filter != null)
                querry = querry.Where(filter);
            if (includeproperties != null)
            {
                foreach (var includeprop in includeproperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
                {
                    querry = querry.Include(includeprop);
                }              

            }
            return querry.FirstOrDefault();
        }

        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderedBy = null, string includeproperties = null)
        {
            
               IQueryable<T> querry = dbset;
                if (filter != null)
                    querry = querry.Where(filter);
                if (includeproperties != null) 
                {
                    foreach (var includeprop in includeproperties.Split(new Char[]{','},StringSplitOptions.RemoveEmptyEntries))
                    {
                    querry = querry.Include(includeprop);
                    }
                }
                if (OrderedBy != null)
                    return OrderedBy(querry).ToList();
                return querry.ToList();          
            
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void Remove(int id)
        {
            //var entity = dbset.Find(id);
            //dbset.Remove(entity);
            var entity = Get(id);
            Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}
