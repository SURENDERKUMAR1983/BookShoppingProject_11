using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.DataAccess.Repository.IRepository
{
   public interface IRepository<T>where T:class
    {
        T Get(int id);
        IEnumerable<T> GetAll(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> OrderedBy = null,
          string includeproperties=null          
            );
        T FirstOrDefault(
            Expression<Func<T,bool>> filter = null,
            string includeproperties = null
            );
        void Add(T entity);
        void Remove(T entity);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> entity);
    }
}
