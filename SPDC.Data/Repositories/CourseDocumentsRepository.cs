using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICourseDocumentsRepository : IRepository<CourseDocument>
    {
        IQueryable<CourseDocument> GetCourseDocumentByQueryable(Expression<Func<CourseDocument, bool>> predicate, string[] includes = null);
    }
    public class CourseDocumentsRepository : RepositoryBase<CourseDocument>, ICourseDocumentsRepository
    {
        public CourseDocumentsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<CourseDocument> GetCourseDocumentByQueryable(Expression<Func<CourseDocument, bool>> predicate, string[] includes = null)
        {
            var query = dbSet.Where(predicate);
            if (includes != null && includes.Count() > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
