using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IEmployerRecommendationsTransTempRepository : IRepository<EmployerRecommendationTempTran>
    {
    }
    public class EmployerRecommendationsTransTempRepository: RepositoryBase<EmployerRecommendationTempTran>, IEmployerRecommendationsTransTempRepository
    {
        public EmployerRecommendationsTransTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
