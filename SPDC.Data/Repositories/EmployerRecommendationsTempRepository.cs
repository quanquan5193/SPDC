using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IEmployerRecommendationsTempRepository : IRepository<EmployerRecommendationTemp>
    {
    }
    public class EmployerRecommendationsTempRepository : RepositoryBase<EmployerRecommendationTemp>, IEmployerRecommendationsTempRepository
    {
        public EmployerRecommendationsTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
