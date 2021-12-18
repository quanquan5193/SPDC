using SPDC.Model.Models.CIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service
{
    public interface ICICService
    {
        void CreateCourse();

        void UpdateCourseStatus();

        void UpdateCourse();

        void CreateApplication();

        Task<UploadAppAttachmentRequestModel> UploadAppAttachment(UploadAppAttachmentRequestModel model);

        void CreatePayment();

        Task<List<GetInstructorResponseModel>> GetInstructor();

        Task<List<GetCentreResponseModel>> GetCentre();

        Task<List<GetOfficerResponseModel>> GetOfficer();

        Task<List<GetVenueResponseModel>> GetVenue();
    }
}
