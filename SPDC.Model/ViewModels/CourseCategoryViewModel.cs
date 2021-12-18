using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CourseCategoryViewModel
    {

        public int Id { get; set; }

        public int Status { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public List<CourseCategorieTranViewModel> CourseCategorieTrans { get; set; }

        public CourseCategoryViewModel()
        {
            Name = CourseCategorieTrans != null ? CourseCategorieTrans.First().Name : "";
        }

    }
}
