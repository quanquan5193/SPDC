using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IAdditionalClassesApprovalRepository : IRepository<AdditionalClassesApproval>
    {

    }
    public class AdditionalClassesApprovalRepository : RepositoryBase<AdditionalClassesApproval> , IAdditionalClassesApprovalRepository
    {
        public AdditionalClassesApprovalRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
