using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IClassAppovedStatusHistoryRepository : IRepository<ClassAppovedStatusHistory>
    {
    }
    public class ClassAppovedStatusHistoryRepository : RepositoryBase<ClassAppovedStatusHistory>, IClassAppovedStatusHistoryRepository
    {
        public ClassAppovedStatusHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
