using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IMakeUpAttendenceRepository : IRepository<MakeUpAttendence>
    {

    }
    public class MakeUpAttendenceRepository : RepositoryBase<MakeUpAttendence>, IMakeUpAttendenceRepository
    {
        public MakeUpAttendenceRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
