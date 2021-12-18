﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class UserDocumentsBindingModel
    {
        [Required]
        public int Id { get; set; }
        public string FileName { get; set; }
    }
}
