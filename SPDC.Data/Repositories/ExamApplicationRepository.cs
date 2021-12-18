﻿using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public class ExamApplicationRepository : RepositoryBase<ExamApplication>, IExamApplicationRepository
    {
        public ExamApplicationRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }
    }
}
