using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IMakeupClassRepository : IRepository<MakeUpClass>
    {

    }
    public class MakeupClassRepository : RepositoryBase<MakeUpClass>, IMakeupClassRepository
    {
        public MakeupClassRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
