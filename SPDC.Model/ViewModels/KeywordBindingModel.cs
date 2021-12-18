using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class KeywordBindingModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string WordEN { get; set; }
        [Required]
        public string WordCN { get; set; }
        [Required]
        public string WordHK { get; set; }
    }
}
