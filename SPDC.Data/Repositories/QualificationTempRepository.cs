using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IQualificationTempRepository : IRepository<QualificationTemp>
    {
    }
    public class QualificationTempRepository : RepositoryBase<QualificationTemp>, IQualificationTempRepository
    {
        public QualificationTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
