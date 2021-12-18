using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IQualificationTransRepository : IRepository<QualificationsTran>
    {
    }
    public class QualificationTransRepository : RepositoryBase<QualificationsTran>, IQualificationTransRepository
    {
        public QualificationTransRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
