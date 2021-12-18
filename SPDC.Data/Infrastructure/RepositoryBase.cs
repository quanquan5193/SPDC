using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Properties
        private ApplicationDbContext dataContext;
        public readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected ApplicationDbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }
        public virtual T Delete(int id)
        {
            var entity = dbSet.Find(id);
            return dbSet.Remove(entity);
        }
        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetSingleById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, string includes)
        {
            return dbSet.Where(where).ToList();
        }


        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            var count = await dataContext.Set<T>().CountAsync(where);
            return count;
        }

        public async Task<List<T>> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                var buildQuery = await query.AsQueryable().ToListAsync();
                return buildQuery;
            }

            var buildQ = await dataContext.Set<T>().AsQueryable().ToListAsync();
            return buildQ;
        }

        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                var buildQuery = await query.FirstOrDefaultAsync(expression);
                return buildQuery;
            }

            var buildQ = await dataContext.Set<T>().FirstOrDefaultAsync(expression);
            return buildQ;
        }

        public async Task<List<T>> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {

            this.dataContext.Database.CommandTimeout = Int32.MaxValue;
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                var buildQuery = await query.Where<T>(predicate).AsQueryable<T>().ToListAsync();
                return buildQuery;
            }

            var buildQ = await dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>().ToListAsync();
            return buildQ;
        }

        public async Task<IEnumerable<T>> GetMulti(Expression<Func<T, bool>> predicate, string sortBy, bool isDescending, string[] includes = null)
        {
            IQueryable<T> _resetSet;

            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dataContext.Set<T>().Where<T>(predicate).AsQueryable() : dataContext.Set<T>().AsQueryable();
            }
            var sortExpression = string.Empty;
            if (!string.IsNullOrEmpty(sortBy))
            {
                sortExpression = sortBy + " " + (isDescending ? "DESC" : "ASC");
            }
            _resetSet = _resetSet.OrderByCustom(sortExpression);
            return await _resetSet.AsQueryable().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetMultiPaging(Expression<Func<T, bool>> predicate, string sortBy, bool isDescending, int index = 0, int size = 20, string[] includes = null)
        {
            int skipCount = index < 0 ? 0 : (index - 1) * size;
            IQueryable<T> _resetSet;

            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dataContext.Set<T>().Where<T>(predicate).AsQueryable() : dataContext.Set<T>().AsQueryable();
            }
            var sortExpression = string.Empty;
            if (!string.IsNullOrEmpty(sortBy))
            {
                sortExpression = sortBy + " " + (isDescending ? "DESC" : "ASC");
            }
            _resetSet = _resetSet.OrderByCustom(sortExpression);
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            return await _resetSet.AsQueryable().ToListAsync();
        }

        public async Task<bool> CheckContains(Expression<Func<T, bool>> predicate)
        {
            int count = await dataContext.Set<T>().CountAsync<T>(predicate);
            return count > 0;
        }
        #endregion
    }
}
