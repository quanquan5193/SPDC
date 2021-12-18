using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IResitExamRepository : IRepository<ResitExam>
    {

    }
    public class ResitExamRepository : RepositoryBase<ResitExam>, IResitExamRepository
    {
        public ResitExamRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
