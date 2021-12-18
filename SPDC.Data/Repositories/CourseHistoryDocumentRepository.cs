using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICourseHistoryDocumentRepository : IRepository<CourseHistoryDocument>
    {
    }
    public class CourseHistoryDocumentRepository : RepositoryBase<CourseHistoryDocument>, ICourseHistoryDocumentRepository
    {
        public CourseHistoryDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
