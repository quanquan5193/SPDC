using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IEmployerRecommendationsRepository : IRepository<EmployerRecommendation>
    {
    }
    public class EmployerRecommendationsRepository : RepositoryBase<EmployerRecommendation>, IEmployerRecommendationsRepository
    {
        public EmployerRecommendationsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
