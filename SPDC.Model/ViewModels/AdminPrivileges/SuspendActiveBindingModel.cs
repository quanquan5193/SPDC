﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class SuspendActiveBindingModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
