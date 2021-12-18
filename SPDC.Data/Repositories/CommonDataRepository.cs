using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICommonDataRepository : IRepository<CommonData>
    {

    }
    public class CommonDataRepository : RepositoryBase<CommonData>, ICommonDataRepository
    {
        public CommonDataRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
