using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchPayment
{
    public class BatchPaymentItemViewModel
    {
        public int UserId { get; set; }

        public string ApplicantName { get; set; }

        public string CicNumber { get; set; }

        public int? ApplicationIdSelected { get; set; }

        public List<CourseAvailable> ListCourseAvailable { get; set; }

        public bool IsChineseName { get; set; }
    }

    public class CourseAvailable
    {
        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public int ApplicationId { get; set; }

        public string AcademicYear { get; set; }

        public decimal? CourseFee { get; set; }

        public int? InvoiceStatus { get; set; }
    }
}
