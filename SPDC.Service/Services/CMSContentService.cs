using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static SPDC.Common.StaticConfig;

namespace SPDC.Service.Services
{
    public interface ICMSContentService
    {
        Task<ResultModel<CmsContentTypeViewModel>> GetListCmsContentType(int langCode);

        Task<ResultModel<CMSBindingModel>> GetCMSContentById(int id);

        Task<ResultModel<CmsContent>> CreateCMSContent(CMSBindingModel model, int userId, HttpPostedFile file = null);

        Task<ResultModel<CmsContent>> UpdateCMSContent(CMSBindingModel model, int userId, HttpPostedFile file = null);

        Task<ResultModel<FileReturnViewModel>> GetImageById(int id);

        Task<ResultModel<bool>> DeleteCmsContent(int id);

        Task<IEnumerable<CmsContent>> GetBackground(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<CmsContent>> GetSideNav(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<CmsContent>> GetBanner(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<CmsContent>> GetAnnouncementDetails(int langId);

        Task<IEnumerable<LandingPageAnnouncementViewModel>> GetLandingPageAnnouncement(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<LandingPageViewModel>> GetLandingPageNewAndEvent(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<LandingPageViewModel>> GetLandingPageHotCourse(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<LandingPageViewModel>> GetLandingPageStemAlliance(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<LandingPageViewModel>> GetLandingPageCareerProgression(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetDisclaimer(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetPrivacyPolicy(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<IEnumerable<CmsContent>> GetFAQ(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetCampusInformation(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetUsefulLinks(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetAboutUs(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetContactUs(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetSitemap(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetInclementWeatherArrangement(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<CmsContent> GetWelcomeMessage(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<ResultModel<bool>> PublishAndUnpublishContent(HandleContentBindingModel handleContents, int modifyBy);

        ResultModel<object> GetLanguageForm(string path);

        int UploadDesctiprionImg(string url, string filename, string type);

        Task<PaginationSet<object>> GetListEvent(int langId, int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<PaginationSet<object>> GetListInclementWeatherArrangements(int langId, int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<PaginationSet<object>> GetListWelcomeMessages(int langId, int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);

        Task<List<CmsContent>> CloneDataToElastic();

        Task<IEnumerable<int>> GetCMSCategoryIdsBySuffixAsync(string suffix);

        Task<CmsContent> GetRawCMSContentById(int id);

        Task<PaginationSet<ContentViewModel>> GetUnMatchContents(string keyword, int contentType, int index, string sortBy, bool isDescending, int size);
        bool MatchContents(int id, int matchId);
        Task<ResultModel<bool>> UpdateCMSPublishStatus();
    }
    public class CMSContentService : ICMSContentService
    {
        private IUnitOfWork _unitOfWork;
        private ICMSContentRepository _cMSContentRepository;
        private ICMSContentTypeRepository _cMSContentTypeRepository;
        private IUserRepository _userRepository;
        private ICMSImageRepository _cMSImageRepository;
        public CMSContentService(IUnitOfWork unitOfWork, ICMSContentRepository cMSContentRepository, ICMSContentTypeRepository cMSContentTypeRepository, IUserRepository userRepository, ICMSImageRepository cMSImageRepository)
        {
            _unitOfWork = unitOfWork;
            _cMSContentRepository = cMSContentRepository;
            _cMSContentTypeRepository = cMSContentTypeRepository;
            _userRepository = userRepository;
            _cMSImageRepository = cMSImageRepository;
        }

        public async Task<ResultModel<CmsContentTypeViewModel>> GetListCmsContentType(int langCode)
        {
            ResultModel<CmsContentTypeViewModel> result = new ResultModel<CmsContentTypeViewModel>();
            var cmsTypes = await _cMSContentTypeRepository.GetAll();
            result.IsSuccess = true;

            var contentTypes = new CmsContentTypeViewModel();
            foreach (var item in cmsTypes)
            {
                var type = (int)DistinguishContentType(item.CmsType);
                if (type == (int)Common.Enums.CmsContentType.BannerAndBackground)
                {
                    contentTypes.BannerBackground.Add(item.ToCmsContentTypeViewModel(langCode));
                }
                else if (type == (int)Common.Enums.CmsContentType.Announcement)
                {
                    contentTypes.Announcement.Add(item.ToCmsContentTypeViewModel(langCode));
                }
                else if (type == (int)Common.Enums.CmsContentType.NewsAndEvents)
                {
                    contentTypes.NewsAndEvents.Add(item.ToCmsContentTypeViewModel(langCode));
                }
                else if (type == (int)Common.Enums.CmsContentType.PromotionalItems)
                {
                    contentTypes.PromotionalItems.Add(item.ToCmsContentTypeViewModel(langCode));
                }
                else if (type == (int)Common.Enums.CmsContentType.OtherInnerPages)
                {
                    contentTypes.OtherInerPages.Add(item.ToCmsContentTypeViewModel(langCode));
                }
                else if (type == (int)Common.Enums.CmsContentType.InclementWeatherArrangement)
                {
                    contentTypes.InclementWeatherArrangements.Add(item.ToCmsContentTypeViewModel(langCode));
                }
                else if (type == (int)Common.Enums.CmsContentType.WelcomeMessage)
                {
                    contentTypes.WelcomeMessages.Add(item.ToCmsContentTypeViewModel(langCode));
                }
            }

            result.Data = contentTypes;
            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<CMSBindingModel>> GetCMSContentById(int id)
        {
            ResultModel<CMSBindingModel> result = new ResultModel<CMSBindingModel>();
            var cms = await _cMSContentRepository.GetSingleByCondition(x => x.Id == id, new string[] { "CmsImages" });
            if (cms == null)
            {
                result.Message = "No content matched";
                result.IsSuccess = true;
                result.Data = null;
                return result;
            }

            var userCreateName = _userRepository.GetSingleById(cms.CreateBy)?.DisplayName;
            var userNameCreate = !String.IsNullOrEmpty(userCreateName) ? userCreateName : "Not Available";
            var userModifiedName = _userRepository.GetSingleById(cms.CreateBy)?.DisplayName;
            var userNameModified = !String.IsNullOrEmpty(userModifiedName) ? userModifiedName : "Not Available";
            var urlImage = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id=";

            string matchedTitle = "";
            if (cms.MatchedItemId != null)
            {
                var matchedCMS = _cMSContentRepository.GetSingleById((int)cms.MatchedItemId);
                matchedTitle = matchedCMS.Title;
            }

            var cmsReturn = cms.ToCMSBindingModel(userCreateName, userNameModified, urlImage, matchedTitle);

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = cmsReturn;
            return result;
        }

        public async Task<ResultModel<CmsContent>> CreateCMSContent(CMSBindingModel model, int userId, HttpPostedFile file = null)
        {
            ResultModel<CmsContent> result = new ResultModel<CmsContent>();
            if (!ValidObject(model))
            {
                result.Message = "Content have not approved yet";
                return result;
            }
            try
            {
                model.FileName = "";
                model.ContentType = "";
                model.ImagePath = "";
                var cmsCotnent = EntityHelpers.ToCmsContent(model, userId);
                if (file != null)
                {
                    var type = _cMSContentTypeRepository.GetSingleById(model.ContentTypeId);
                    var folder = DistinguishContentType(type.CmsType).ToString();

                    var directory = ConfigHelper.GetByKey("ContentUpload");
                    var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                    var path = serPath + "\\" + folder;
                    if (!Directory.Exists(path))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(path);
                    }
                    var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + file.FileName);
                    file.SaveAs(pathFile);

                    model.FileName = file.FileName;
                    model.ContentType = file.ContentType;
                    model.ImagePath = folder + "\\" + file.FileName;
                }
                if (!String.IsNullOrEmpty(model.FileName))
                {
                    cmsCotnent.CmsImages.Add(new CmsImage()
                    {
                        Url = model.ImagePath,
                        FileName = model.FileName,
                        ContentType = model.ContentType
                    });
                }

                _cMSContentRepository.Add(cmsCotnent);
                _unitOfWork.Commit();

                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = cmsCotnent;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<ResultModel<CmsContent>> UpdateCMSContent(CMSBindingModel model, int userId, HttpPostedFile file = null)
        {
            ResultModel<CmsContent> result = new ResultModel<CmsContent>();
            if (!ValidObject(model))
            {
                result.Message = "Content have not approved yet";
                return result;
            }
            var cms = await _cMSContentRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "CmsImages" });

            if (cms == null)
            {
                result.Message = "No CMS content matched";
                return result;
            }

            try
            {
                model.FileName = "";
                model.ContentType = "";
                model.ImagePath = "";
                var type = _cMSContentTypeRepository.GetSingleById(model.ContentTypeId);
                var enumType = DistinguishContentType(type.CmsType);
                //if (enumType == Common.Enums.CmsContentType.BannerAndBackground && file == null)
                //{
                //    result.Message = "This content required an image";
                //    return result;
                //}
                var folder = enumType.ToString();
                var cmsCotnent = EntityHelpers.ToCmsContent(model, userId, cms);
                if (file != null)
                {
                    var directory = ConfigHelper.GetByKey("ContentUpload");
                    var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                    var path = serPath + folder;
                    //Delete old file
                    var oldFilePath = serPath + "\\" + cms.CmsImages.FirstOrDefault()?.Url;
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }

                    //Create new file
                    if (!Directory.Exists(path))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(path);
                    }
                    var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + file.FileName);
                    file.SaveAs(pathFile);

                    model.FileName = file.FileName;
                    model.ContentType = file.ContentType;
                    model.ImagePath = folder + "\\" + file.FileName;
                }
                if (!String.IsNullOrEmpty(model.FileName))
                {
                    _cMSImageRepository.Delete(cms.CmsImages.FirstOrDefault());
                    cms.CmsImages.Add(new CmsImage()
                    {
                        Url = model.ImagePath,
                        FileName = model.FileName,
                        ContentType = model.ContentType
                    });
                }


                #region CheckOrderNumber

                //// Update old ordered record
                List<CmsContent> cmsOrders = new List<CmsContent>();

                if (cmsCotnent.CmsStatus == (int)CMSPublishType.Publish)
                {
                    var cmsContentType = _cMSContentTypeRepository.GetSingleById(cmsCotnent.ContentTypeId);

                    int[] applyNumbers = new int[] { };

                    if (model.ApplyingFor == (int)ContentApplyingFor.Both)
                    {
                        applyNumbers = new int[] { (int)ContentApplyingFor.Both, (int)ContentApplyingFor.Website, (int)ContentApplyingFor.Mobile };
                    }
                    else if (model.ApplyingFor == (int)ContentApplyingFor.Website)
                    {
                        applyNumbers = new int[] { (int)ContentApplyingFor.Both, (int)ContentApplyingFor.Website };
                    }
                    else
                    {
                        applyNumbers = new int[] { (int)ContentApplyingFor.Both, (int)ContentApplyingFor.Mobile };
                    }

                    if (cmsCotnent.ShowOnLandingPage)
                    {
                        cmsOrders = await _cMSContentRepository.GetMulti(x => x.OrderNumber == cmsCotnent.OrderNumber
                                                               && x.CmsContentType.CmsType.Equals(cmsContentType.CmsType)
                                                               && x.CmsStatus == (int)CMSPublishType.Publish
                                                               && x.Id != cmsCotnent.Id
                                                               && x.ShowOnLandingPage
                                                               && applyNumbers.Contains(x.ApplyingFor));
                    }


                    foreach (var item in cmsOrders)
                    {
                        // set order number to default null
                        item.OrderNumber = 0;
                        _cMSContentRepository.Update(item);
                        //_unitOfWork.Commit();

                    }
                }
                #endregion

                _cMSContentRepository.Update(cmsCotnent);
                _unitOfWork.Commit();

                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = cms;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<ResultModel<FileReturnViewModel>> GetImageById(int id)
        {
            ResultModel<FileReturnViewModel> result = new ResultModel<FileReturnViewModel>();
            var image = await _cMSImageRepository.GetSingleByCondition(x => x.Id == id);
            MemoryStream stream;
            if (image == null)
            {
                return GetDefaultImage();
            }

            var directory = ConfigHelper.GetByKey("ContentUpload");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            var path = serPath + image.Url;

            if (File.Exists(path))
            {
                stream = new MemoryStream(File.ReadAllBytes(path));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileType = image.ContentType,
                    FileName = image.FileName
                };
            }
            else
            {
                return GetDefaultImage();
            }

            return result;
        }

        private ResultModel<FileReturnViewModel> GetDefaultImage()
        {
            var result = new ResultModel<FileReturnViewModel>();
            result.Message = "Image not found";
            result.IsSuccess = false;
            var defaultPath = System.Web.HttpContext.Current.Server.MapPath("~/images/default/default.jpg");
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(File.ReadAllBytes(defaultPath));
            }
            catch (Exception ex)
            {
                throw;
            }
            result.Data = new FileReturnViewModel()
            {
                Stream = stream,
                FileType = "image/png",
                FileName = "default.jpg"
            };
            return result;
        }

        private Common.Enums.CmsContentType DistinguishContentType(string content)
        {
            var value = StaticEnum.GetValueFromAttribute<Common.Enums.CmsContentType, StringValueAttribute>(content, x => x.StringValue);
            return value;
        }

        public async Task<ResultModel<bool>> DeleteCmsContent(int id)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            try
            {
                var cmsContent = await _cMSContentRepository.GetSingleByCondition(x => x.Id == id, new string[] { "CmsImages" });
                if (cmsContent != null)
                {

                    if (cmsContent.CmsImages.Count > 0)
                    {
                        var directory = ConfigHelper.GetByKey("ContentUpload");
                        var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                        foreach (var item in cmsContent.CmsImages)
                        {
                            var oldFilePath = serPath + "\\" + item.Url;
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }
                        _cMSImageRepository.DeleteMulti(x => x.CmsId == id);
                    }

                    _cMSContentRepository.Delete(cmsContent);
                    _unitOfWork.Commit();
                    result.Message = "Success";
                    result.IsSuccess = true;
                    result.Data = true;
                    return result;
                }
                result.Message = "Content not found";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
                throw;
            }

        }

        private bool ValidObject(CMSBindingModel model)
        {
            return model.ApproveStatus == false && model.PublishStatus == true ? false : true;
        }

        public async Task<IEnumerable<CmsContent>> GetBackground(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            //var result = await _cMSContentRepository.GetMulti(
            //    x => x.CmsContentType.Id == 1 && x.ShowOnLandingPage == true && x.ApproveStatus == true && x.PublishStatus == true
            //    , new string[] { "CmsImages" });
            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == 1 
                     && x.ShowOnLandingPage == true 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && ((x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both) || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     , new string[] { "CmsImages" });
            return result;
        }
        public async Task<IEnumerable<CmsContent>> GetSideNav(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            //var result = await _cMSContentRepository.GetMulti(
            //       x => x.CmsContentType.Id == 2 && x.ShowOnLandingPage == true && x.ApproveStatus == true && x.PublishStatus == true
            //       , new string[] { "CmsImages" });
            var result = await _cMSContentRepository.GetMultiPaging(
                    x => x.CmsContentType.Id == 2 
                    && x.ShowOnLandingPage == true 
                    && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                    && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                    , "LastPublishDate"
                    , true
                    , 1
                    , 1
                    , new string[] { "CmsImages" });
            return result;

        }
        public async Task<IEnumerable<CmsContent>> GetBanner(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 3;
            if (langId == (int)LanguageCode.CN)
                type = 4;
            if (langId == (int)LanguageCode.HK)
                type = 5;

            var orderResult = (await _cMSContentRepository.GetMultiPaging(
                     x => x.OrderNumber != null && x.OrderNumber > 0 
                     && x.CmsContentType.Id == type 
                     && x.ShowOnLandingPage == true 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "OrderNumber"
                     , false
                     , 1
                     , 5
                     , new string[] { "CmsImages" })).ToList();

            if (orderResult.Count() < 5)
            {
                var result = await _cMSContentRepository.GetMultiPaging(
                         x => (x.OrderNumber == null || x.OrderNumber == 0) 
                         && x.CmsContentType.Id == type 
                         && x.ShowOnLandingPage == true 
                         && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                         && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                         , "LastPublishDate"
                         , true
                         , 1
                         , 5 - orderResult.Count()
                         , new string[] { "CmsImages" });
                orderResult.AddRange(result);
            }
            return orderResult;
        }

        //TO DELETE
        public async Task<IEnumerable<CmsContent>> GetAnnouncementDetails(int langId)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 6;
            if (langId == (int)LanguageCode.HK)
                type = 7;
            if (langId == (int)LanguageCode.CN)
                type = 8;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     , "LastPublishDate"
                     , true
                     , 1
                     , 5
                     );
            return result;
        }

        public async Task<IEnumerable<LandingPageAnnouncementViewModel>> GetLandingPageAnnouncement(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 6;
            if (langId == (int)LanguageCode.HK)
                type = 7;
            if (langId == (int)LanguageCode.CN)
                type = 8;

            var result = await _cMSContentRepository.GetAllContentAndSort(type, applyingFor);

            var lstReusult = new List<LandingPageAnnouncementViewModel>();
            foreach (var item in result)
            {
                lstReusult.Add(item.ToLandingPageAnnouncementViewModel());
            }

            return lstReusult;
        }

        public async Task<IEnumerable<LandingPageViewModel>> GetLandingPageNewAndEvent(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int[] type = null;
            if (langId == (int)LanguageCode.EN)
                type = new int[2] { 9, 12 };
            if (langId == (int)LanguageCode.HK)
                type = new int[2] { 10, 13 };
            if (langId == (int)LanguageCode.CN)
                type = new int[2] { 11, 14 };

            //var result = await _cMSContentRepository.GetMultiPaging(
            //         x => type.Contains(x.CmsContentType.Id) && x.ApproveStatus == true && x.PublishStatus == true
            //         && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
            //         , "LastPublishDate"
            //         , true
            //         , 1
            //         , 5
            //         );

            var orderResult = (await _cMSContentRepository.GetMultiPaging(
                     x => x.OrderNumber != null && x.OrderNumber > 0 
                     && type.Contains(x.CmsContentType.Id) 
                     && x.ShowOnLandingPage == true
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "OrderNumber"
                     , false
                     , 1
                     , 5
                     , new string[] { "CmsImages" })).ToList();

            if (orderResult.Count() < 5)
            {
                var result = await _cMSContentRepository.GetMultiPaging(
                         x => (x.OrderNumber == null || x.OrderNumber == 0) 
                         && type.Contains(x.CmsContentType.Id) 
                         && x.ShowOnLandingPage == true 
                         && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                         && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                         , "LastPublishDate"
                         , true
                         , 1
                         , 5 - orderResult.Count()
                         , new string[] { "CmsImages" });
                orderResult.AddRange(result);
            }

            var lstReusult = new List<LandingPageViewModel>();
            foreach (var item in orderResult)
            {
                lstReusult.Add(item.ToLandingPageViewModel());
            }
            return lstReusult;
        }

        public async Task<IEnumerable<LandingPageViewModel>> GetLandingPageHotCourse(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 15;
            if (langId == (int)LanguageCode.HK)
                type = 16;
            if (langId == (int)LanguageCode.CN)
                type = 17;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            var lstReusult = new List<LandingPageViewModel>();
            foreach (var item in result)
            {
                lstReusult.Add(item.ToLandingPageViewModel());
            }
            return lstReusult;
        }

        public async Task<IEnumerable<LandingPageViewModel>> GetLandingPageStemAlliance(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 18;
            if (langId == (int)LanguageCode.HK)
                type = 19;
            if (langId == (int)LanguageCode.CN)
                type = 20;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            var lstReusult = new List<LandingPageViewModel>();
            foreach (var item in result)
            {
                lstReusult.Add(item.ToLandingPageViewModel());
            }
            return lstReusult;
        }

        public async Task<IEnumerable<LandingPageViewModel>> GetLandingPageCareerProgression(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 21;
            if (langId == (int)LanguageCode.HK)
                type = 22;
            if (langId == (int)LanguageCode.CN)
                type = 23;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            var lstReusult = new List<LandingPageViewModel>();
            foreach (var item in result)
            {
                lstReusult.Add(item.ToLandingPageViewModel());
            }
            return lstReusult;
        }

        public async Task<CmsContent> GetDisclaimer(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 24;
            if (langId == (int)LanguageCode.HK)
                type = 25;
            if (langId == (int)LanguageCode.CN)
                type = 26;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<CmsContent> GetPrivacyPolicy(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 27;
            if (langId == (int)LanguageCode.HK)
                type = 28;
            if (langId == (int)LanguageCode.CN)
                type = 29;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<CmsContent>> GetFAQ(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 30;
            if (langId == (int)LanguageCode.HK)
                type = 31;
            if (langId == (int)LanguageCode.CN)
                type = 32;
            var lstContent = await _cMSContentRepository.GetMulti(x => x.CmsContentType.Id == type 
                                                                    && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                                                                    && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both));
            var result = lstContent.OrderBy(x => x.LastPublishDate ?? DateTime.Now);

            return result;
        }

        public async Task<CmsContent> GetCampusInformation(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 33;
            if (langId == (int)LanguageCode.HK)
                type = 34;
            if (langId == (int)LanguageCode.CN)
                type = 35;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<CmsContent> GetUsefulLinks(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 36;
            if (langId == (int)LanguageCode.HK)
                type = 37;
            if (langId == (int)LanguageCode.CN)
                type = 38;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<CmsContent> GetAboutUs(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 39;
            if (langId == (int)LanguageCode.HK)
                type = 40;
            if (langId == (int)LanguageCode.CN)
                type = 41;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<CmsContent> GetContactUs(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 42;
            if (langId == (int)LanguageCode.HK)
                type = 43;
            if (langId == (int)LanguageCode.CN)
                type = 44;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<ResultModel<bool>> PublishAndUnpublishContent(HandleContentBindingModel handleContents, int modifyBy)
        {
            var result = new ResultModel<bool>();
            var cmsContent = await _cMSContentRepository.GetMulti(x => handleContents.ListIdContent.Contains(x.Id), new string[] { "CmsContentType" });

            #region CheckOrderNumber
            if (handleContents.IsPublish)
            {
                foreach (var model in cmsContent)
                {
                    int[] applyNumbers = new int[] { };

                    if (model.ApplyingFor == (int)ContentApplyingFor.Both)
                    {
                        applyNumbers = new int[] { (int)ContentApplyingFor.Both, (int)ContentApplyingFor.Website, (int)ContentApplyingFor.Mobile };
                    }
                    else if (model.ApplyingFor == (int)ContentApplyingFor.Website)
                    {
                        applyNumbers = new int[] { (int)ContentApplyingFor.Both, (int)ContentApplyingFor.Website };
                    }
                    else
                    {
                        applyNumbers = new int[] { (int)ContentApplyingFor.Both, (int)ContentApplyingFor.Mobile };
                    }

                    IEnumerable<int> preOrderPublishContents = null;
                    preOrderPublishContents = cmsContent.Where(x => x.OrderNumber == model.OrderNumber
                                                       && x.CmsContentType.CmsType.Equals(model.CmsContentType.CmsType)
                                                       && (x.CmsStatus == (int)CMSPublishType.Approve || x.CmsStatus == (int)CMSPublishType.Unpublish)
                                                       && x.ShowOnLandingPage
                                                       && applyNumbers.Contains(x.ApplyingFor)).Select(x => x.Id);

                    if (preOrderPublishContents != null && preOrderPublishContents.Count() > 1)
                    {
                        result.Message = "Incorrect slider sequence, please check again.";
                        return result;
                    }

                    List<CmsContent> cmsOrders = new List<CmsContent>();
                    // Update old ordered record

                    if (model.ShowOnLandingPage)
                    {
                        cmsOrders = await _cMSContentRepository.GetMulti(x => x.OrderNumber == model.OrderNumber
                                                                        && x.CmsContentType.CmsType.Equals(model.CmsContentType.CmsType)
                                                                        && x.CmsStatus == (int)CMSPublishType.Publish
                                                                        && x.Id != model.Id
                                                                        && x.ShowOnLandingPage
                                                                        && applyNumbers.Contains(x.ApplyingFor));
                    }
                    foreach (var cmsOrder in cmsOrders)
                    {
                        if (cmsOrder != null && cmsOrder.Id != model.Id && handleContents.IsPublish)
                        {
                            // set order number to default null
                            cmsOrder.OrderNumber = 0;
                            _cMSContentRepository.Update(cmsOrder);
                        }
                    }
                }
            }
            #endregion

            foreach (var item in cmsContent)
            {
                if (item.ApproveStatus == true)
                {
                    item.PublishStatus = handleContents.IsPublish;
                    item.CmsStatus = (int)(handleContents.IsPublish ? CMSPublishType.Publish : CMSPublishType.Unpublish);
                    item.LastPublishDate = DateTime.Now;
                    item.LastModifiedBy = modifyBy;
                    _cMSContentRepository.Update(item);
                }
                else
                {
                    result.Message = $"This content Id = {item.Id} is not approved";
                    return result;
                }
            }
            _unitOfWork.Commit();
            result.Message = "Success";
            result.IsSuccess = true;
            return result;

        }

        public ResultModel<object> GetLanguageForm(string path)
        {
            var result = new ResultModel<object>();
            try
            {
                if (File.Exists(path))
                {
                    var textJson = File.ReadAllText(path);
                    object jsonArray = JsonConvert.DeserializeObject<object>(textJson);
                    result.Data = jsonArray;
                    result.Message = "Success";
                    result.IsSuccess = true;
                    return result;
                }
                else
                {
                    result.Message = "File Json is not exist";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public int UploadDesctiprionImg(string url, string filename, string type)
        {
            CmsImage image = new CmsImage()
            {
                Url = url,
                ContentType = type,
                FileName = filename
            };
            var result = _cMSImageRepository.Add(image);
            _unitOfWork.Commit();
            return result.Id;
        }

        public async Task<PaginationSet<object>> GetListEvent(int langId, int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            List<int> type = null;
            if (langId == (int)LanguageCode.EN)
                type = new List<int>() { 9, 12 };
            if (langId == (int)LanguageCode.HK)
                type = new List<int>() { 10, 13 };
            if (langId == (int)LanguageCode.CN)
                type = new List<int>() { 11, 14 };

            var cmsItems = (await _cMSContentRepository.GetMultiPaging(
                     x => type.Contains(x.CmsContentType.Id) 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , index
                     , size
                     )).Select(i => new
                     {
                         Id = i.Id,
                         ShortDescription = i.ShortDescription,
                         SEOUrlLink = i.SEOUrlLink
                     });
            var total = await _cMSContentRepository.Count(x => type.Contains(x.CmsContentType.Id)
                                                            && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                                                            && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both));
            PaginationSet<object> result = new PaginationSet<object>();
            result.Page = index;
            result.Items = cmsItems;
            result.TotalCount = total;
            return result;
        }

        public async Task<PaginationSet<object>> GetListInclementWeatherArrangements(int langId, int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            List<int> type = null;
            if (langId == (int)LanguageCode.EN)
                type = new List<int>() { 45 };
            if (langId == (int)LanguageCode.HK)
                type = new List<int>() { 46 };
            if (langId == (int)LanguageCode.CN)
                type = new List<int>() { 47 };

            var cmsItems = (await _cMSContentRepository.GetMultiPaging(
                     x => type.Contains(x.CmsContentType.Id) 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , index
                     , size
                     )).Select(i => new
                     {
                         Id = i.Id,
                         ShortDescription = i.ShortDescription,
                         SEOUrlLink = i.SEOUrlLink
                     });
            var total = await _cMSContentRepository.Count(x => type.Contains(x.CmsContentType.Id) 
                                                        && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                                                        && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both));
            PaginationSet<object> result = new PaginationSet<object>();
            result.Page = index;
            result.Items = cmsItems;
            result.TotalCount = total;
            return result;
        }


        public async Task<PaginationSet<object>> GetListWelcomeMessages(int langId, int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            List<int> type = null;
            if (langId == (int)LanguageCode.EN)
                type = new List<int>() { 51 };
            if (langId == (int)LanguageCode.HK)
                type = new List<int>() { 52 };
            if (langId == (int)LanguageCode.CN)
                type = new List<int>() { 53 };

            var cmsItems = (await _cMSContentRepository.GetMultiPaging(
                     x => type.Contains(x.CmsContentType.Id) 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , index
                     , size
                     )).Select(i => new
                     {
                         Id = i.Id,
                         ShortDescription = i.ShortDescription,
                         SEOUrlLink = i.SEOUrlLink
                     });
            var total = await _cMSContentRepository.Count(x => type.Contains(x.CmsContentType.Id) 
                            && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                            && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both));
            PaginationSet<object> result = new PaginationSet<object>();
            result.Page = index;
            result.Items = cmsItems;
            result.TotalCount = total;
            return result;
        }

        public async Task<List<CmsContent>> CloneDataToElastic()
        {
            var lst = await _cMSContentRepository.GetMulti(x => x.ContentTypeId >= 6 && x.ContentTypeId <= 23, new string[] { "CmsContentType", "CmsImages" });
            return lst;
        }

        public async Task<IEnumerable<int>> GetCMSCategoryIdsBySuffixAsync(string suffix)
        {
            var lst = await _cMSContentTypeRepository.GetMulti(x => x.Id > 2 && x.Name.EndsWith(suffix));
            return lst.Select(x => x.Id);
        }

        public async Task<CmsContent> GetRawCMSContentById(int id)
        {
            var cms = await _cMSContentRepository.GetSingleByCondition(x => x.Id == id, new string[] { "CmsImages" });

            return cms;
        }

        public async Task<CmsContent> GetSitemap(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 48;
            if (langId == (int)LanguageCode.HK)
                type = 49;
            if (langId == (int)LanguageCode.CN)
                type = 50;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<CmsContent> GetInclementWeatherArrangement(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 45;
            if (langId == (int)LanguageCode.HK)
                type = 46;
            if (langId == (int)LanguageCode.CN)
                type = 47;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }
        public async Task<CmsContent> GetWelcomeMessage(int langId, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            int type = 0;
            if (langId == (int)LanguageCode.EN)
                type = 51;
            if (langId == (int)LanguageCode.HK)
                type = 52;
            if (langId == (int)LanguageCode.CN)
                type = 53;

            var result = await _cMSContentRepository.GetMultiPaging(
                     x => x.CmsContentType.Id == type 
                     && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                     && (x.ApplyingFor == applyingFor || x.ApplyingFor == (int)Common.Enums.ContentApplyingFor.Both)
                     , "LastPublishDate"
                     , true
                     , 1
                     , 1
                     );
            return result.FirstOrDefault();
        }

        public async Task<PaginationSet<ContentViewModel>> GetUnMatchContents(string keyword, int contentType, int index, string sortBy, bool isDescending, int size)
        {
            var cmses = await _cMSContentRepository.GetMultiPaging(x => x.ContentTypeId == contentType && x.Title.Contains(keyword) && x.MatchedItemId == null && x.CmsStatus == (int)CMSPublishType.Publish, sortBy, isDescending, index, size, new string[] { "CmsContentType" });
            var total = await _cMSContentRepository.Count(x => x.ContentTypeId == contentType && x.Title.Equals(keyword) && x.MatchedItemId == null && x.CmsStatus == (int)CMSPublishType.Publish);

            PaginationSet<ContentViewModel> result = new PaginationSet<ContentViewModel>();
            result.Page = index;
            result.Items = cmses.Select(
                i => new ContentViewModel()
                {
                    ContentID = i.Id,
                    Title = i.Title,
                    DateCreated = i.CreateDate,
                    ContentType = i.CmsContentType != null ? i.CmsContentType.Name : "",
                    AnnoucementDate = i.AnnoucementDate,
                    LastPublishDate = i.LastPublishDate,
                    ApprovalStatus = i.ApproveStatus ? "Approved" : "Unapproved",
                    PublishStatus = i.PublishStatus ? "Published" : "Unpublished",
                    UrlImage = "",
                    Description = i.Description,
                    ShortDescription = i.ShortDescription,
                    CmsStatus = i.CmsStatus
                });
            result.TotalCount = total;

            return result;
        }

        public bool MatchContents(int id, int matchId)
        {
            try
            {
                var cms1 = _cMSContentRepository.GetSingleById(id);
                var cms2 = _cMSContentRepository.GetSingleById(matchId);

                cms1.MatchedItemId = matchId;
                cms2.MatchedItemId = id;

                _cMSContentRepository.Update(cms1);
                _cMSContentRepository.Update(cms2);

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ResultModel<bool>> UpdateCMSPublishStatus()
        {
            var result = new ResultModel<bool>();
            var today = DateTime.Now;

            var listCMSRealese = (await _cMSContentRepository.GetMulti(x => x.ApproveStatus == true && x.ReleaseDate.HasValue && (x.ReleaseDate ?? DateTime.Now).Day == today.Day
                && (x.ReleaseDate ?? DateTime.Now).Month == today.Month && (x.ReleaseDate ?? DateTime.Now).Year == today.Year
            ));

            listCMSRealese.ForEach(c =>
            {
                c.PublishStatus = true;
                _cMSContentRepository.Update(c);
            });

            var listCMSEnd = (await _cMSContentRepository.GetMulti(x => x.ApproveStatus == true && x.EndDate.HasValue && (x.EndDate ?? DateTime.Now).Day == today.Day
                && (x.EndDate ?? DateTime.Now).Month == today.Month && (x.EndDate ?? DateTime.Now).Year == today.Year
            ));

            listCMSEnd.ForEach(c =>
            {
                c.PublishStatus = false;
                _cMSContentRepository.Update(c);
            });

            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }
    }
}
