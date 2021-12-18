using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPDC.Model.Models.CIC
{
    public class UpdateCourseStatusRequestModel
    {
        [Required]
        public string Course_Code { get; set; }

        [Required]
        public string Status_Code { get; set; }

        [Required]
        public string UpdatedBy { get; set; }
    }
}