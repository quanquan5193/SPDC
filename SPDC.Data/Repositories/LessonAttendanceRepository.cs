using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ILessonAttendanceRepository : IRepository<LessonAttendance>
    {

    }
    public class LessonAttendanceRepository : RepositoryBase<LessonAttendance>, ILessonAttendanceRepository
    {
        public LessonAttendanceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
