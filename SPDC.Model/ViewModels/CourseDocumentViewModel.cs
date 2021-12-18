using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CourseDocumentViewModel
    {
        public CourseDocumentViewModel()
        {
            ListCourseBrochure = new List<SubCourseDocument>();
            ListApplicationForm = new List<SubCourseDocument>();
            ListDocumentDetail = new List<DocumentViewModel>();
        }
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public List<SubCourseDocument> ListCourseBrochure { get; set; }
        public List<SubCourseDocument> ListApplicationForm { get; set; }
        public List<DocumentViewModel> ListDocumentDetail { get; set; }
    }

    public class SubCourseDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }

    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
