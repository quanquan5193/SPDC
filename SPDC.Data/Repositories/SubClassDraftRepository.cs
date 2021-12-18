using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ISubClassDraftRepository : IRepository<SubClassDraft>
    {
    }
    public class SubClassDraftRepository : RepositoryBase<SubClassDraft>, ISubClassDraftRepository
    {
        public SubClassDraftRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
