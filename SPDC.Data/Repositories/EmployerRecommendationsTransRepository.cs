using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IEmployerRecommendationsTransRepository : IRepository<EmployerRecommendationTran>
    {
    }
    public class EmployerRecommendationsTransRepository: RepositoryBase<EmployerRecommendationTran>, IEmployerRecommendationsTransRepository
    {
        public EmployerRecommendationsTransRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
