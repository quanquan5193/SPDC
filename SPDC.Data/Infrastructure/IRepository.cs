using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        T Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        T Delete(T entity);

        T Delete(int id);

        //Delete multi records
        void DeleteMulti(Expression<Func<T, bool>> where);

        // Get an entity by int id
        T GetSingleById(int id);

        Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        Task<List<T>> GetAll(string[] includes = null);

        Task<List<T>> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        Task<IEnumerable<T>> GetMulti(Expression<Func<T, bool>> predicate, string sortBy, bool isDescending, string[] includes = null);

        Task<IEnumerable<T>> GetMultiPaging(Expression<Func<T, bool>> filter, string sortBy, bool isDescending, int index = 0, int size = 20, string[] includes = null);

        Task<int> Count(Expression<Func<T, bool>> where);

        Task<bool> CheckContains(Expression<Func<T, bool>> predicate);
    }
}
