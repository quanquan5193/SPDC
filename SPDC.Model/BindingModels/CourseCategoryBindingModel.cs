using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class CourseCategoryBindingModel
    {
        [Required]
        public int Status { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public List<CourseCategoryValue> values { get; set; }

    }

    public class CourseCategoryValue
    {
        public int LanguageId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Title { get; set; }
    }
}
