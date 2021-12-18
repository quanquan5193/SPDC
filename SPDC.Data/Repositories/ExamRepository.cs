using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{
    public interface IExamRepository : IRepository<Exam>
    {

    }
    public class ExamRepository : RepositoryBase<Exam>, IExamRepository
    {
        public ExamRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
