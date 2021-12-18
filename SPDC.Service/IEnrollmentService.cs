using SPDC.Common;
using SPDC.Model.ViewModels.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service
{
    public interface IEnrollmentService
    {
        Task<IList<EnrollmentCalendarViewModel>> GetCalendar(int userId, DateTime? from, DateTime? to);

        Task<PaginationSet<EnrollmentMyClassViewModel>> GetClasses(int userId, int index, int pageSize);

        Task<EnrollmentDetailViewModel> GetEnrollmentDetail(int appplicationId);

        Task<PaginationSet<EnrollmentClassDetailViewModel>> GetClassDetail(int classId, int index, int pageSize);

        Task<PaginationSet<EnrollmentExamDetailViewModel>> GetExams(int applicationId, int index, int pageSize);

        Task<IList<EnrollmentInvoiceReceiptViewModel>> GetInvoices(int applicationId);
    }
}
