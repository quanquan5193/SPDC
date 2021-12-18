using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.ApplicationManagement;
using SPDC.Model.BindingModels.Assessment;
using SPDC.Model.BindingModels.BatchPayment;
using SPDC.Model.BindingModels.Invoice;
using SPDC.Model.BindingModels.MakeupClass;
using SPDC.Model.BindingModels.StudentAndClassManagement;
using SPDC.Model.BindingModels.Transaction;
using SPDC.Model.BindingModels.Transaction.RefundTransaction;
using SPDC.Model.Models;
using SPDC.Model.ViewModels.Application;
using SPDC.Model.ViewModels.ApplicationManagement;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.ViewModels.BatchPayment;
using SPDC.Model.ViewModels.Invoice;
using SPDC.Model.ViewModels.MakeupClass;
using SPDC.Model.ViewModels.Transaction;
using SPDC.Service.Services;
using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using static SPDC.Common.StaticConfig;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/ApplicationManagement")]
    [EnableCors("*", "*", "*")]
    public class ApplicationManagementController : ApiControllerBase
    {
        private IStudentClassManagementService _studentClassManagementService;
        private IMakeupClassService _makeupClassService;
        private IClientService _clientService;
        private IDocumentService _documentService;
        private ICourseService _courseService;
        private IApplicationService _applicationService;
        private IClassService _classService;

        public ApplicationManagementController(
            IStudentClassManagementService studentClassManagementService,
            IMakeupClassService makeupClassService,
            IClientService clientService,
            IDocumentService documentService,
            ICourseService courseService,
            IApplicationService applicationService,
            IClassService classService)
        {
            _studentClassManagementService = studentClassManagementService;
            _makeupClassService = makeupClassService;
            _clientService = clientService;
            _documentService = documentService;
            _courseService = courseService;
            _applicationService = applicationService;
            _classService = classService;
        }

        [HttpPost]
        [Route("ListApplication")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ListApplicationByFilter(StudentClassManageFilterBindingModel model)
        {
            var result = await _studentClassManagementService.ListApplicationByFilter(model);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("ApplicationDetail")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ApplicationDetailById(int id)
        {
            var result = await _studentClassManagementService.ApplicationDetailById(id, (int)GetLanguageCode());

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("ApprovedHistory")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListApplicationeApprovedHistoryByCourseId(ApplicationHistoryFilter filter)
        {
            var result = await _studentClassManagementService.GetListApplicationeApprovedHistoryByCourseId(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("ApprovedApplication")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ChangeApprovedStatusApplication()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<ApprovedRejectedApplicationBindingModel>(HttpContext.Current.Request.Params["ApprovalData"]);

            var files = HttpContext.Current.Request.Files;
            var result = await _studentClassManagementService.ChangeApprovedStatusApplication(model, files, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("download-document")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DownloadApplicationApprovalDocument(int docId)
        {
            Log.Info($"Start Download Application Approval Document (Admin) - Id: {docId}");
            if (docId == 0)
            {
                Log.Error($"Failed Download Application Approval Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Document Id can not be 0"));
            }

            var result = await _studentClassManagementService.DownloadApplicationApprovalDocument(docId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download Application Approval Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Application Approval Document (Admin) - Id: {docId}");
            return httpResponseMessage;
        }

        [HttpPost]
        [Route("AssignedClass")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> AssignedClassForApplication(AssignedClassBindingModel model)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _studentClassManagementService.AssignedClassForApplication(model, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("Replace/{applicationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ReplaceApplication(int applicationId)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var result = await _studentClassManagementService.ReplaceApplication(applicationId, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("SummaryTable")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ViewSummaryTable(SummaryTableFilter filter)
        {
            var result = await _studentClassManagementService.ViewSummaryTable(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("BankCodeBankname")]
        public async Task<IHttpActionResult> GetBankCodeBankName()
        {
            var result = await _studentClassManagementService.GetBankCodeBankName();

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("CreateInvoice")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateInvoice(InvoiceCreateBindingModel model)
        {
            int id = 0;
            //var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            //if (!success)
            //{
            //    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            //}

            var result = model.InvoiceId == 0 ? await _studentClassManagementService.CreateInvoice(model, id) : await _studentClassManagementService.EditInvoice(model, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));

        }

        [HttpGet]
        [Route("OfferPayment")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> SendInvoiceOffered(int invoiceId)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _studentClassManagementService.SendInvoiceOffered(invoiceId, id);
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("DownloadInvoiceAttachment/{invoiceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> DownloadInvoiceAttachment(int invoiceId)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            Log.Info($"Start Download Invoice Attachment (Admin) - Id: {invoiceId}");
            if (invoiceId == 0)
            {
                Log.Error($"Failed Download Invoice Attachment (Admin) - Id: {invoiceId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Document Id can not be 0"));
            }

            var result = await _studentClassManagementService.DownloadInvoiceAttachment(invoiceId, id);
            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download Application Approval Document (Admin) - Id: {invoiceId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Invoice Attachment (Admin) - Id: {invoiceId}");
            return httpResponseMessage;
        }

        [HttpPost]
        [Route("WaivedHistory")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListWaiverApprovedHistory(WaivedHisrotyFilter filter)
        {
            var result = await _studentClassManagementService.GetListWaiverApprovedHistory(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("ApproveWaivedPayment")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ChangeWaivedPaymentApprovalStatus()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<WaivedPaymentApprovedHistoryBindingModel>(HttpContext.Current.Request.Params["ApprovalData"]);
            var files = HttpContext.Current.Request.Files;

            var result = await _studentClassManagementService.ChangeWaivedPaymentApprovedStatus(model, files, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("RefundedInvoice")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ChangeInvoiceToRefunded(int invoiceId)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _studentClassManagementService.ChangeInvoiceToRefunded(invoiceId, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("CheckInvoiceOverdue")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> CheckInvoiceOverdue()
        {
            var result = await _studentClassManagementService.CheckInvoiceOverdue();

            var httpResponseMessage = result.IsSuccess ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.BadRequest);
            return httpResponseMessage;
        }

        #region Transaction
        [HttpPost]
        [Route("CreateEditTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> CreateTransaction()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<TransactionBindingModel>(HttpContext.Current.Request.Params["TransactionModel"]);
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "Object not valid", Data = null });
            }
            var files = HttpContext.Current.Request.Files;

            var result = model.Id == 0 ? await _studentClassManagementService.CreateTransaction(model, files, id) : await _studentClassManagementService.EditTransaction(model, files, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));

        }

        [HttpPost]
        [Route("GetTransactions")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListTransaction(TransactionFilter filter)
        {
            var result = await _studentClassManagementService.GetListTransaction(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("DownloadTransaction")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DownloadTransactionDocument(int docId, int typeFileTransaction)
        {
            Log.Info($"Start Download Application Approval Document (Admin) - Id: {docId}");
            if (docId == 0)
            {
                Log.Error($"Failed Download Application Approval Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Document Id can not be 0"));
            }

            var result = typeFileTransaction == (int)SPDC.Common.Enums.TypeFileTransaction.CreateBySelf ? await _studentClassManagementService.DownloadTransactionDocument(docId) :
                await _studentClassManagementService.DownloadDocument(docId, ConfigHelper.GetByKey("BatchPaymentDirectory"));

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download Application Approval Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Application Approval Document (Admin) - Id: {docId}");
            return httpResponseMessage;
        }

        [HttpGet]
        [Route("DeleteTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteTransactionById(int transId)
        {
            var result = await _studentClassManagementService.DeleteTransactionById(transId);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("GetTransactionById")]
        public async Task<IHttpActionResult> GetTransactionById(int transId)
        {
            var result = await _studentClassManagementService.GetTransactionById(transId);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("EditRefundTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> EditRefundTransaction()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<TransactionBindingModel>(HttpContext.Current.Request.Params["RefundTransactionModel"]);
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "Object not valid", Data = null });
            }

            var files = HttpContext.Current.Request.Files;

            var result = await _studentClassManagementService.EditRefundTransaction(model, files, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("GetRefundTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetRefundTransactionById(int transId)
        {
            var result = await _studentClassManagementService.GetRefundTransactionById(transId);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("ApprovedRefundTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ChangeApprovedStatusRefundTransaction()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<ApprvovedRefundTransactionBindingModel>(HttpContext.Current.Request.Params["ApprovedRejectedModel"]);

            var files = HttpContext.Current.Request.Files;
            var result = await _studentClassManagementService.ChangeApprovedStatusRefundTransaction(id, model, files);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("ListRefundAprrovedHistory")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListRefundTrasactionById(RefundTransactionFilter model)
        {
            var result = await _studentClassManagementService.GetListRefundTrasactionById(model);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("DeleteRefundTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteRefundTransactionById(int transId)
        {
            var result = await _studentClassManagementService.DeleteRefundTransactionById(transId);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        #endregion

        #region Batch Application
        [HttpGet]
        [Route("BatchApplicationTemplate")]
        [AllowAnonymous]//TODO: Set Admin permission
        public async Task<HttpResponseMessage> DownloadBatchApplicationTemplate()
        {
            var directory = ConfigHelper.GetByKey("BatchPaymentTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

            if (!File.Exists(serPath))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Template was not found"));
            }

            var stream = new MemoryStream(File.ReadAllBytes(serPath));
            var fileName = "SPDC_Batch_Application_Template_v1.xlsx";

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            return httpResponseMessage;
        }

        [HttpPost]
        [Route("BatchApplicationUpload")]
        [AllowAnonymous] //TODO: Set Admin permission
        public async Task<IHttpActionResult> BatchApplicationUpload()
        {

            #region Check Admin permission
            //int id = 0;
            //var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);

            //if (!success)
            //{
            //    Log.Error($"Failed Upload File");
            //    return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = "", Data = null });
            //}
            //var user = await .FindByIdAsync(id);
            //if (user == null)
            //{
            //    Log.Error($"Failed Upload File");
            //    return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            //}
            #endregion

            var files = HttpContext.Current.Request.Files.GetMultiple("BatchApplicationFiles");

            if (files.Count == 0)
            {
                Log.Error($"BatchApplicationUpload : No file uploaded");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("No file uploaded"));
            }

            for (int i = 0; i < files.Count; i++)
            {
                // Read file excel
                if (!files[i].ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                {
                    Log.Error($"BatchApplicationUpload : File must be excel (.xlsx)");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel("File must be excel (.xlsx)"));
                }
                if (files[i].ContentLength > 5000000)
                {
                    Log.Error($"BatchApplicationUpload : File must be smaller then 5mb");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel("File must be smaller then 5mb"));
                }
            }

            var result = await _studentClassManagementService.BatchApplicationUpload(files);
            return Ok(result);
        }
        #endregion

        #region Batch Payment
        [HttpPost]
        [Route("ListBatPayment")]
        public async Task<IHttpActionResult> GetListBatchPaymentByPeriodTime(BatchPaymentFilter filter)
        {
            var result = await _studentClassManagementService.GetListBatchPaymentByPeriodTime(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }
        /// <summary>
        /// Get User For Settlled By CICNumber
        /// </summary>
        /// <param name="cicNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ListUserByCICNumber")]
        public async Task<IHttpActionResult> GetListUserByCICNumber(string cicNumber)
        {
            var result = await _studentClassManagementService.GetListUserByCICNumber(cicNumber);

            return Content(HttpStatusCode.OK, result);
        }
        /// <summary>
        /// Get User For Settlled By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ListUserByName")]
        public async Task<IHttpActionResult> GetListUserByName(string name)
        {
            var result = await _studentClassManagementService.GetListUserByName(name);

            return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Get Batch Payment Item By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isChineseName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BatchPaymentItem")]
        public async Task<IHttpActionResult> GetUserApplicationsByUserId(int userId, bool isChineseName)
        {
            var result = await _studentClassManagementService.GetUserApplicationsByUserId(userId, isChineseName);

            return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Create Batch Payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUpdateBatchPayment")]
        public async Task<IHttpActionResult> CreateUpdateBatchPayment()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<BatchPaymentDetailBindingModel>(HttpContext.Current.Request.Params["BatchPaymentModel"]);

            bool isDupplicateApplication = false;
            if (model.ListApplication != null && model.ListApplication.Count > 0)
            {
                isDupplicateApplication = model.ListApplication.GroupBy(x => x.ApplicationId).Any(c => c.Count() > 1);
            }

            if (isDupplicateApplication)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Duplicate applied course for same student. Please check."));
            }

            var files = HttpContext.Current.Request.Files;

            var result = model.Id == 0 ? await _studentClassManagementService.CreateBatchPayment(model, id, files) : await _studentClassManagementService.UpdateBatchPayment(model, id, files);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("GetBatchPaymentDetail")]
        public async Task<IHttpActionResult> GetBatchPaymentDetailById(int batchPaymentId)
        {
            var result = await _studentClassManagementService.GetBatchPaymentDetailById(batchPaymentId);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("DownloadBatchPayment")]
        public async Task<HttpResponseMessage> DownloadBatchPaymentFile(int docId, string kindOfFile)
        {
            var result = await _studentClassManagementService.DownloadDocument(docId, kindOfFile);


            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Document - Id: {docId}");
            return httpResponseMessage;
        }

        [HttpPost]
        [Route("SettledAll")]
        public async Task<IHttpActionResult> SettledAllBatchPayment()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var aaa = HttpContext.Current.Request.Params["BatchPaymentModel"];

            var model = new JavaScriptSerializer().Deserialize<BatchPaymentDetailBindingModel>(aaa);

            var isDupplicateApplication = model.ListApplication.GroupBy(x => x.ApplicationId).Any(c => c.Count() > 1);

            if (isDupplicateApplication)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("ApplicationId is dupplicated"));
            }

            var files = HttpContext.Current.Request.Files;

            var result = model.Id == 0 ? await _studentClassManagementService.CreateBatchPayment(model, id, files) : await _studentClassManagementService.UpdateBatchPayment(model, id, files);

            if (result.IsSuccess == false)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
            }

            var isSettledAll = await _studentClassManagementService.SettledAllBatchPayment(result.Data.Id, id);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data)) :
                Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, result.Data));

        }

        [HttpGet]
        [Route("Demo")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Demo()
        {
            var dic = new List<ApplicationForBatchPayment>();
            dic.Add(new ApplicationForBatchPayment() { ApplicationId = 1, IsChineseName = false });
            dic.Add(new ApplicationForBatchPayment() { ApplicationId = 2, IsChineseName = true });
            dic.Add(new ApplicationForBatchPayment() { ApplicationId = 3, IsChineseName = false });
            var ser = new JavaScriptSerializer().Serialize(dic);


            return Content(HttpStatusCode.OK, new ActionResultModel(ser));
        }


        #endregion

        #region Attendance Managenebt function

        #region Makeup Class

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("MakeupClass")]
        public async Task<IHttpActionResult> CreateEditMakeupClass()
        {
            int makeupClassId = 0;

            var files = HttpContext.Current.Request.Files.GetMultiple("MakeupClassFiles");
            var model = JsonConvert.DeserializeObject<MakeupClassBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["MakupClassBindingModel"]));
            var deleteFiles = JsonConvert.DeserializeObject<int[]>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["DeleteFileIds"]));
            if (model.Id == 0)
            {
                makeupClassId = _makeupClassService.Create(model, files);
            }
            else
            {
                makeupClassId = await _makeupClassService.UpdateAsync(model, files, deleteFiles);
            }

            if (makeupClassId > 0)
            {
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success_create", "MakeupClass", GetLanguageCode().ToString()), true, makeupClassId));
            }

            return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("success_failure", "MakeupClass", GetLanguageCode().ToString())));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("GetMakeupClass")]
        public async Task<IHttpActionResult> GetMakeupClasses(MakeupClassSearchModel model)
        {
            string apiPath = await _clientService.GetClientUrlByNameAsync("ApiPortal");
            List<MakeUpClass> result = await _makeupClassService.GetMakeUpClassesAsync(model);
            List<MakeupClassViewModel> rtnLst = result.ToMakeUpClassViewModels(apiPath);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success_create", "MakeupClass", GetLanguageCode().ToString()), true, rtnLst));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetMakeupClassDetail/{id}")]
        public async Task<IHttpActionResult> GetMakeupClassDetal(int id)
        {
            string apiPath = await _clientService.GetClientUrlByNameAsync("ApiPortal");
            MakeUpClass result = await _makeupClassService.GetMakeUpClassesByIdAsync(id);
            MakeupClassViewModel model = result.ToMakeUpClassViewModel(apiPath);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success_create", "MakeupClass", GetLanguageCode().ToString()), true, model));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("DownloadMakeupClassDoc/{docId}")]
        public async Task<HttpResponseMessage> DownloadMakeupClassDoc(int docId)
        {
            Log.Info($"Start Download Make-up Class Document (Admin) - Id: {docId}");
            if (docId == 0)
            {
                Log.Error($"Failed Download Make-up Class Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = _documentService.GetDocumentById(docId);

            if (result == null)
            {
                Log.Error($"Failed Download Make-up Class Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var pathDoc = System.Web.HttpContext.Current.Server.MapPath(result.Url);

            if (!File.Exists(pathDoc))
            {
                Log.Error($"Failed Download Make-up Class Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var stream = new MemoryStream(File.ReadAllBytes(pathDoc));
            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.ContentType);

            Log.Info($"Completed Download Make-up Class Document (Admin) - Id: {docId}");
            return httpResponseMessage;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/CourseCode")]
        public async Task<IHttpActionResult> SearchCourseCode(string key = "")
        {
            IEnumerable<Model.ViewModels.Application.CourseCodeViewModel> result = await _courseService.GetCourseCodeByText(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                result = (await _courseService.GetCourses()).Select(x => new CourseCodeViewModel()
                {
                    Id = x.Id,
                    CourseCode = x.CourseCode,
                    AcademicYear = x.TargetClasses?.AcademicYear ?? "",
                    CourseNameCN = x.CourseTrans.FirstOrDefault(y => y.LanguageId == (int)LanguageCode.HK)?.CourseName ?? "",
                    CourseNameEN = x.CourseTrans.FirstOrDefault(y => y.LanguageId == (int)LanguageCode.EN)?.CourseName ?? "",
                    AcademicYears = new string[] { x.TargetClasses?.AcademicYear ?? "" }
                });
            }
            else
            {
                result = await _courseService.GetCourseCodeByText(key);
            }

            return Content(HttpStatusCode.OK, new ActionResultModel("", true, result));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/Student")]
        public async Task<IHttpActionResult> SearchStudent(string no = "", string namecn = "", string nameen = "")
        {
            int paramsFlag = 0;
            if (!string.IsNullOrWhiteSpace(no))
                paramsFlag++;
            if (!string.IsNullOrWhiteSpace(namecn))
                paramsFlag++;
            if (!string.IsNullOrWhiteSpace(nameen))
                paramsFlag++;
            if (paramsFlag != 1)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Please send 1 param."));
            }

            var result = await _applicationService.GetApplicationForMakeupClass(no, namecn, nameen);
            return Content(HttpStatusCode.OK, new ActionResultModel("", true, result));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/Lesson")]
        public async Task<IHttpActionResult> SearchLesson(int appid = 0, string classcode = "")
        {

            if (!string.IsNullOrWhiteSpace(classcode) && appid != 0)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Please send 1 param."));
            }

            IEnumerable<AssignLessonViewModel> result;
            if (appid != 0)
            {
                result = await _applicationService.GetAssignLesson(appid);
                return Content(HttpStatusCode.OK, new ActionResultModel("", true, result));
            }

            if (!string.IsNullOrWhiteSpace(classcode))
            {
                result = await _classService.GetAssignLesson(classcode);
                return Content(HttpStatusCode.OK, new ActionResultModel("", true, result));
            }

            return Content(HttpStatusCode.BadRequest, new ActionResultModel("Please send 1 param."));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/AssignToMakeupClass")]
        public async Task<IHttpActionResult> AssignToMakeupClass(AssignToMakeupClassBindingModel models)
        {
            var id = 0;
            int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);

            var result = await _makeupClassService.AssignStudentToMakeupClass(models, id);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        #endregion

        #region Assign Student to Make-up Lesson

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/AssignApplications")]
        public async Task<IHttpActionResult> GetAssignApplications(int[] ids)
        {
            var result = await _applicationService.GetAssignApplications(ids);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/Class")]
        public async Task<IHttpActionResult> SearchClassCode(string coursecode = "")
        {
            IEnumerable<ClassCodeViewModel> result;

            if (!string.IsNullOrWhiteSpace(coursecode))
            {
                result = await _courseService.GetClassCodeByCourseCode(coursecode);
                return Content(HttpStatusCode.OK, new ActionResultModel("", true, result));
            }

            return Content(HttpStatusCode.BadRequest, new ActionResultModel("Please send 1 param."));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/AssignToMakeUpLesson")]
        public async Task<IHttpActionResult> AssignToMakeUpLesson(AssignToMakeupLessonBindingModel models)
        {
            if (!(await _makeupClassService.AssignStudentValidation(models)))
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Duplicated lesson is assigned. Please check again."));
            }

            var id = 0;
            int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);

            var result = await _makeupClassService.AssignStudentsToMakeupLesson(models, id);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        #endregion

        #region Attendance 

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/Search")]
        public async Task<IHttpActionResult> SearchAttendance(AttendanceSearchBindingModel models)
        {
            string apiPath = await _clientService.GetClientUrlByNameAsync("ApiPortal");
            var result = await _makeupClassService.SearchAllAttendance(models, apiPath);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Attendance/Save")]
        public async Task<IHttpActionResult> SaveAttendance()
        {
            var models = JsonConvert.DeserializeObject<IEnumerable<AttendanceViewModel>>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["AttendanceViewModels"]));
            var deleteFiles = JsonConvert.DeserializeObject<int[]>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["DeleteFileIds"]));
            HttpFileCollection files = HttpContext.Current.Request.Files;

            //List<HttpPostedFile> files = HttpContext.Current.Request.Files.GetMultiple("MakeupClassFiles").ToList();
            var result = await _makeupClassService.SaveAllAttendance(models, files, deleteFiles);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Attendance/CheckCourseHasExamination/{courseId}")]
        public async Task<IHttpActionResult> CheckCourseHasExamination(int courseId)
        {
            bool isCourseHasExamination = await _courseService.CheckCourseHasExamination(courseId);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, isCourseHasExamination));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Attendance/SendIneligibleForExamEmail/{courseId}")]
        public async Task<IHttpActionResult> SendIneligibleForExamEmail(int courseId, int[] ids)
        {
            bool isCourseHasExamination = await _courseService.CheckCourseHasExamination(courseId);
            if (!isCourseHasExamination)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("The course has not examination"));
            }

            var result = await _applicationService.SendIneligibleForExamEmail(courseId, ids);
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        #endregion

        #endregion

        #region Assessment
        [HttpGet]
        [Route("GetAssessmentByClassId")]
        public async Task<IHttpActionResult> GetAssessmentListByClassId(int classId)
        {
            var result = await _studentClassManagementService.GetAssessmentListByClassId(classId);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("UpdateAssessment")]
        public async Task<IHttpActionResult> UpdateAssessment()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var lstModel = JsonConvert.DeserializeObject<List<AssessmentRecordBindingModel>>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["ListAssessmentRecord"]));
            var files = HttpContext.Current.Request.Files;

            var result = await _studentClassManagementService.UpdateAssessment(lstModel, files, id);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        #region Button Assessment
        [HttpPost]
        [Route("SendIneligibleForAttendanceCertificateEmail")]
        public async Task<IHttpActionResult> SendIneligibleForAttendanceCertificateEmail(List<int> lstApplicationId)
        {
            var result = await _studentClassManagementService.SendIneligibleForAttendanceCertificateEmail(lstApplicationId, GetLanguageCode());

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) :
                Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("SendCollectAttendanceCertificationEmail")]
        public async Task<IHttpActionResult> SendCollectAttendanceCertificationEmail(List<int> lstApplicationId)
        {
            var result = await _studentClassManagementService.SendCollectAttendanceOrCourseCompletionEmail(lstApplicationId, GetLanguageCode(), Common.Enums.TypeAssessmentEmail.CollectAttendance);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) :
                Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("SendCollectCourseCompletionCertificationEmail")]
        public async Task<IHttpActionResult> SendCollectCourseCompletionCertificationEmail(List<int> lstApplicationId)
        {
            var result = await _studentClassManagementService.SendCollectAttendanceOrCourseCompletionEmail(lstApplicationId, GetLanguageCode(), Common.Enums.TypeAssessmentEmail.CollectCourseCompletion);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) :
                Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("SendExamResult")]
        public async Task<IHttpActionResult> SendExamResult(List<int> lstApplicationId, int examType)
        {
            var result = await _studentClassManagementService.SendExamResult(lstApplicationId, GetLanguageCode(), examType);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) :
                Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("SendReExamDetail")]
        public async Task<IHttpActionResult> SendReExamDetail([FromBody] IEnumerable<int> lstApplicationId, int examType)
        {
            var result = await _studentClassManagementService.SendReExamDetail(lstApplicationId, GetLanguageCode(), examType);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) :
                Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        #endregion

        #region Assign Other ReExam
        [HttpGet]
        [Route("GetAssignReExamFrom")]
        public async Task<IHttpActionResult> GetAssignReExamFrom(int classId)
        {
            var result = await _studentClassManagementService.GetAssignReExamFrom(classId);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("AssignReExamTimeslot")]
        public async Task<IHttpActionResult> AssignReExamTimeslot(AssignReExamTimeslotBindingModel model)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (model.ExamOriginalId == model.ExamDestinationId)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Re-Exam From and Re-Exam To can not be the same Id"));
            }

            var isDupplicateApplication = model.ListApplicationId.GroupBy(x => x).Any(c => c.Count() > 1);
            if (isDupplicateApplication)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Duplicated re-exam timeslot is assigned. Please check again."));
            }

            var result = await _studentClassManagementService.AssignReExamTimeslot(model);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess))
                : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess));
        }
        #endregion

        #endregion



        #region Add resit exam class

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Route("ResitExam")]
        public IHttpActionResult CreateEditResitExam(ResitExamBindingModel model)
        {
            if (model.Type == (int)ExamType.Exam)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Failure"));
            }

            int resitExamId = _applicationService.CreateEditResitExam(model);

            return Content(HttpStatusCode.OK, new ActionResultModel("Success", true, new { Id = resitExamId }));
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Route("GetResitExam")]
        public async Task<IHttpActionResult> GetResitExam(ResitExamSearchModel model)
        {
            IEnumerable<ResitExamViewModel> result = await _applicationService.GetResitExames(model);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success_create", "MakeupClass", GetLanguageCode().ToString()), true, result));
        }


        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Route("ResitExamDetail/{id}")]
        public async Task<IHttpActionResult> GetResitExam(int id)
        {
            ResitExamViewModel result = await _applicationService.GetResitExamesById(id);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success_create", "MakeupClass", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Route("AssignToReExamTimeslot")]
        public async Task<IHttpActionResult> AssignToReExamTimeslot(AssignToReExamTimeslotBindingModel models)
        {
            bool result = await _applicationService.AssignToReExamTimeslot(models);
            return Content(HttpStatusCode.OK, new ActionResultModel("Success", true, result));
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Route("ReExamClass")]
        public async Task<IHttpActionResult> GetResitExam(string studentNo)
        {
            Application result = await _applicationService.GetApplicationByStudentNo(studentNo);
            List<AssignReExamViewModel> rtnList = new List<AssignReExamViewModel>();
            foreach (var examApp in result.ExamApplications)
            {
                rtnList.Add(new AssignReExamViewModel()
                {
                    ExamId = examApp.Exam.Id,
                    ClassCode = examApp.Exam.Class.ClassCode,
                    Date = examApp.Exam.Date.ToString("dd/MM/yyyy"),
                    FromTime = examApp.Exam.FromTime,
                    ToTime = examApp.Exam.ToTime
                });
            }

            return Content(HttpStatusCode.OK, new ActionResultModel("Success", true, rtnList));
        }

        #endregion
    }


}
