﻿using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IWaiverApprovedStatusHistoryRepository : IRepository<WaiverApprovedStatusHistory>
    {
    }
    public class WaiverApprovedStatusHistoryRepository : RepositoryBase<WaiverApprovedStatusHistory>, IWaiverApprovedStatusHistoryRepository
    {
        public WaiverApprovedStatusHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
