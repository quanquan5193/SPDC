using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{
    public interface IClassCommonRepository : IRepository<ClassCommon>
    { }

    public class ClassCommonRepository : RepositoryBase<ClassCommon>, IClassCommonRepository
    {
        public ClassCommonRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
