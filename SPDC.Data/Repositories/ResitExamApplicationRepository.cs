using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IResitExamApplicationRepository : IRepository<ResitExamApplication>
    {

    }
    public class ResitExamApplicationRepository : RepositoryBase<ResitExamApplication>, IResitExamApplicationRepository
    {
        public ResitExamApplicationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
