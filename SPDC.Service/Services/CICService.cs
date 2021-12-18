using RestSharp;
using SPDC.Model.Models.CIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SPDC.Service.Services
{
    public class CICService : ICICService
    {
        public CICService()
        {

        }

        #region DELETE Method



        #endregion

        #region PUT Method

        public void UpdateCourse()
        {
            throw new NotImplementedException();
        }

        public void UpdateCourseStatus()
        {
            throw new NotImplementedException();
        }

        public async Task<UploadAppAttachmentRequestModel> UploadAppAttachment(UploadAppAttachmentRequestModel model)
        {
            var result = await ApiHelper.Instance().Put<UploadAppAttachmentRequestModel, UploadAppAttachmentRequestModel>("api/Proxy?url=SPDC/ApplicationAttachment", model);
            return result;
        }

        #endregion

        #region POST Method

        public void CreateApplication()
        {
            throw new NotImplementedException();
        }

        public void CreateCourse()
        {
            throw new NotImplementedException();
        }

        public void CreatePayment()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GET Method

        public async Task<List<GetCentreResponseModel>> GetCentre()
        {
            var result = await ApiHelper.Instance().Get<List<GetCentreResponseModel>>("api/Proxy?url=SPDC/Centre", new List<KeyValuePair<string, string>> { });
            return result;
        }

        public async Task<List<GetInstructorResponseModel>> GetInstructor()
        {
            var result = await ApiHelper.Instance().Get<List<GetInstructorResponseModel>>("api/Proxy?url=SPDC/Instructor", new List<KeyValuePair<string, string>> { });
            return result;
        }

        public async Task<List<GetOfficerResponseModel>> GetOfficer()
        {
            var result = await ApiHelper.Instance().Get<List<GetOfficerResponseModel>>("api/Proxy?url=SPDC/Officer", new List<KeyValuePair<string, string>> { });
            return result;
        }

        public async Task<List<GetVenueResponseModel>> GetVenue()
        {
            var result = await ApiHelper.Instance().Get<List<GetVenueResponseModel>>("api/Proxy?url=SPDC/GetVenue", new List<KeyValuePair<string, string>> { });

            return result;
        }

        #endregion
    }
}
