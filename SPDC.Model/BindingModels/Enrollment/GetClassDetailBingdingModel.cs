﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Enrollment
{
    public class GetClassDetailBingdingModel
    {
        public int ClassId { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
    }
}
