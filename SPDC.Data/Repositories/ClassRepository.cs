using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface IClassRepository : IRepository<Class>
    {

    }

    public class ClassRepository : RepositoryBase<Class>, IClassRepository
    {
        public ClassRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
