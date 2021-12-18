using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IMediumOfInstructionRepository : IRepository<MediumOfInstruction>
    {
    }

    public class MediumOfInstructionRepository : RepositoryBase<MediumOfInstruction>, IMediumOfInstructionRepository
    {
        public MediumOfInstructionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
