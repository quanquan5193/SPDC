﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.EmailConfirmation
{
    public class EmailConfirmationBindingModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
