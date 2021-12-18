using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{
    public interface ILessionRepository : IRepository<Lesson>
    {

    }
    public class LessionRepository : RepositoryBase<Lesson>, ILessionRepository
    {
        public LessionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
