using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface IEnrollmentStatusRepository : IRepository<EnrollmentStatus>
    {

    }

    public class EnrollmentStatusRepository : RepositoryBase<EnrollmentStatus>, IEnrollmentStatusRepository
    {
        public EnrollmentStatusRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}

