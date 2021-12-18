using AutoMapper;
using SPDC.Common;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPDC.Model.BindingModels;
using System.Web.Script.Serialization;
using SPDC.Model.ViewModels.Notification;
using SPDC.Model.ViewModels.AdditionalClassApproval;

namespace SPDC.WebAPI.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<ApplicationUser, UserViewModel>();
            Mapper.CreateMap<ApplicationUser, AdminViewModel>();

            Mapper.CreateMap<CourseCategorieTran, CourseCategorieTranViewModel>();
            Mapper.CreateMap<CourseCategory, CourseCategoryViewModel>()
                .ForMember(d => d.CourseCategorieTrans, m => m.MapFrom(p => p.CourseCategorieTrans))
                .ForMember(d => d.Name, m => m.MapFrom(p => (p.CourseCategorieTrans.Count > 0 ? p.CourseCategorieTrans.First().Name : string.Empty)));
            Mapper.CreateMap<ApplicationUser, AccountInfoViewModel>()
                .ForMember(d => d.OtherContactEmail, m => m.MapFrom(p => p.OtherEmail))
                .ForMember(d => d.CICNo, m => m.MapFrom(p => p.CICNumber))
                .ForMember(d => d.InterestedCourse, m => m.MapFrom(p => p.Particular.InterestedTypeOfCourse != null ? new JavaScriptSerializer().Deserialize<List<int>>(p.Particular.InterestedTypeOfCourse) : null))
                ;
            Mapper.CreateMap<ParticularBindingModel, Particular>()
                .ForMember(d => d.HKIDNo, m => m.MapFrom(p => !String.IsNullOrEmpty(p.HKIDNo) ? EncryptUtilities.EncryptAes256(p.HKIDNo) : null))
                .ForMember(d => d.MobileNumber, m => m.MapFrom(p => !String.IsNullOrEmpty(p.MobileNumber) ? EncryptUtilities.EncryptAes256(p.MobileNumber) : null))
                .ForMember(d => d.PassportNo, m => m.MapFrom(p => !String.IsNullOrEmpty(p.PassportNo) ? EncryptUtilities.EncryptAes256(p.PassportNo) : null));

            Mapper.CreateMap<ApplicationUser, UserDataViewModel>()
                .ForMember(d => d.ID, m => m.MapFrom(p => p.Id))
                .ForMember(d => d.Name, m => m.MapFrom(p => p.UserName))
                .ForMember(d => d.Email, m => m.MapFrom(p => p.Email));

            Mapper.CreateMap<Class, ClassViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(p => p.Id))
                .ForMember(d => d.ClassCode, m => m.MapFrom(p => p.ClassCode))
                /*.ForMember(d => d.AcademicYear, m => m.MapFrom(p => p.AcademicYear))*/;

            //Mapper Class Model
            Mapper.CreateMap<ClassCommon, ClassCommonBindingModel>();
            Mapper.CreateMap<ClassCommonBindingModel, ClassCommon>();
            Mapper.CreateMap<ClassCommonBindingModel, ClassCommonViewModel>();
            Mapper.CreateMap<ClassCommonViewModel, ClassCommonBindingModel>();

            //map target class model
            Mapper.CreateMap<TargetClassViewModel, TargetClassBindingModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
                .ForMember(d => d.CourseId, m => m.MapFrom(s => s.CourseId))
                .ForMember(d => d.TargetNumberClass, m => m.MapFrom(s => s.TargetNumberClass))
                .ForMember(d => d.ClassBindingModels, m => m.MapFrom(s => s.ClassViewDetailModels))
                .ForMember(d => d.ClassCommonBindingModel, m => m.MapFrom(s => s.ClassCommonViewModel))
                ;

            Mapper.CreateMap<TargetClassBindingModel, TargetClasses>()
                //.ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                //.ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
                .ForMember(d => d.Id, m => m.MapFrom(s => s.CourseId)) // Mapping courseid to id
                .ForMember(d => d.TargetNumberClass, m => m.MapFrom(s => s.TargetNumberClass))
                ;

            Mapper.CreateMap<TargetClasses, TargetClassBindingModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                //.ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
                //.ForMember(d => d.CourseId, m => m.MapFrom(s => s.CourseId))
                .ForMember(d => d.TargetNumberClass, m => m.MapFrom(s => s.TargetNumberClass))
                //.ForMember(d => d.ClassBindingModels, m => m.MapFrom(s => s.Classes))
                //.ForMember(d => d.ClassCommonBindingModel, m => m.MapFrom(s => s.ClassCommon))
                ;

            Mapper.CreateMap<TargetClassBindingModel, TargetClassViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
                .ForMember(d => d.CourseId, m => m.MapFrom(s => s.CourseId))
                .ForMember(d => d.TargetNumberClass, m => m.MapFrom(s => s.TargetNumberClass))
                .ForMember(d => d.ClassViewDetailModels, m => m.MapFrom(s => s.ClassBindingModels))
                .ForMember(d => d.ClassCommonViewModel, m => m.MapFrom(s => s.ClassCommonBindingModel))
                ;

            //map class
            Mapper.CreateMap<Class, ClassBindingModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.ClassCode, m => m.MapFrom(s => s.ClassCode))
                .ForMember(d => d.CourseId, m => m.MapFrom(s => s.CourseId))
                .ForMember(d => d.CommencementDate, m => m.MapFrom(s => s.CommencementDate.ToUniversalTime()))
                .ForMember(d => d.CompletionDate, m => m.MapFrom(s => s.CompletionDate))
                .ForMember(d => d.IsExam, m => m.MapFrom(s => s.IsExam))
                .ForMember(d => d.IsReExam, m => m.MapFrom(s => s.IsReExam))
                .ForMember(d => d.ExamPassingMask, m => m.MapFrom(s => s.ExamPassingMask))
                .ForMember(d => d.ReExamFees, m => m.MapFrom(s => s.ReExamFees))
                //.ForMember(d => d.AcademicYear, m => m.MapFrom(s => s.AcademicYear))
                .ForMember(d => d.InvisibleOnWebsite, m => m.MapFrom(s => s.InvisibleOnWebsite))
                .ForMember(d => d.AttendanceRequirement, m => m.MapFrom(s => s.AttendanceRequirement))
                .ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
                .ForMember(d => d.EnrollmentNumber, m => m.MapFrom(s => s.EnrollmentNumber))
                .ForMember(d => d.Capacity, m => m.MapFrom(s => s.Capacity))
                //.ForMember(d => d.TargetClassId, m => m.MapFrom(s => s.TargetClassId))
                .ForMember(d => d.CountReExam, m => m.MapFrom(s => s.CountReExam))
                .ForMember(d => d.Lessons, m => m.MapFrom(s => s.Lessons))
                .ForMember(d => d.Exams, m => m.MapFrom(s => s.Exams))
                .ForMember(d => d.SubClassStatus, m => m.MapFrom(s => s.SubClassStatus))
                ;
            Mapper.CreateMap<ClassBindingModel, Class>();

            Mapper.CreateMap<ClassBindingModel, ClassViewModel>()
               .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
               .ForMember(d => d.ClassCode, m => m.MapFrom(s => s.ClassCode))
               .ForMember(d => d.AcademicYear, m => m.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.AttendanceRequirement, m => m.MapFrom(s => s.AttendanceRequirement))
               .ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
               .ForMember(d => d.EnrollmentNumber, m => m.MapFrom(s => s.EnrollmentNumber))
               .ForMember(d => d.CommencementDate, m => m.MapFrom(s => s.CommencementDate))
               .ForMember(d => d.CompletionDate, m => m.MapFrom(s => s.CompletionDate))
               .ForMember(d => d.Capacity, m => m.MapFrom(s => s.Capacity))
               .ForMember(d => d.SubClassStatus, m => m.MapFrom(s => s.SubClassStatus))
               .ForMember(d => d.SubClassApprovedStatus, m => m.MapFrom(s => s.SubClassApprovedStatus))
               .ForMember(d => d.InvisibleOnWebsite, m => m.MapFrom(s => s.InvisibleOnWebsite))
               ;

            Mapper.CreateMap<ClassBindingModel, ClassViewDetailModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.ClassCode, m => m.MapFrom(s => s.ClassCode))
                .ForMember(d => d.CourseId, m => m.MapFrom(s => s.CourseId))
                .ForMember(d => d.CommencementDate, m => m.MapFrom(s => s.CommencementDate))
                .ForMember(d => d.CompletionDate, m => m.MapFrom(s => s.CompletionDate))
                .ForMember(d => d.IsExam, m => m.MapFrom(s => s.IsExam))
                .ForMember(d => d.IsReExam, m => m.MapFrom(s => s.IsReExam))
                .ForMember(d => d.ExamPassingMask, m => m.MapFrom(s => s.ExamPassingMask))
                .ForMember(d => d.ReExamFees, m => m.MapFrom(s => s.ReExamFees))
                .ForMember(d => d.AcademicYear, m => m.MapFrom(s => s.AcademicYear))
                .ForMember(d => d.InvisibleOnWebsite, m => m.MapFrom(s => s.InvisibleOnWebsite))
                .ForMember(d => d.AttendanceRequirement, m => m.MapFrom(s => s.AttendanceRequirement))
                .ForMember(d => d.ClassCommonId, m => m.MapFrom(s => s.ClassCommonId))
                .ForMember(d => d.EnrollmentNumber, m => m.MapFrom(s => s.EnrollmentNumber))
                .ForMember(d => d.Capacity, m => m.MapFrom(s => s.Capacity))
                .ForMember(d => d.TargetClassId, m => m.MapFrom(s => s.TargetClassId))
                .ForMember(d => d.Lessons, m => m.MapFrom(s => s.Lessons))
                .ForMember(d => d.Exams, m => m.MapFrom(s => s.Exams.Where(x => x.IsReExam == false)))
                .ForMember(d => d.FirstReExam, m => m.MapFrom(s => s.Exams.Where(x => x.IsReExam && x.Type == (int)Common.Enums.ExamType.FirstReExam)))
                .ForMember(d => d.SecondReExams, m => m.MapFrom(s => s.Exams.Where(x => x.IsReExam && x.Type == (int)Common.Enums.ExamType.SecondReExam)))
                ;

            //map lesson
            Mapper.CreateMap<LessonBindingModel, Lesson>();
            Mapper.CreateMap<Lesson, LessonBindingModel>();
            Mapper.CreateMap<LessonBindingModel, LessonViewModel>();
            Mapper.CreateMap<LessonViewModel, LessonBindingModel>();

            //map exam
            Mapper.CreateMap<ExamBindingModel, Exam>();
            Mapper.CreateMap<Exam, ExamBindingModel>();
            Mapper.CreateMap<ExamBindingModel, ExamViewModel>();
            Mapper.CreateMap<ExamViewModel, ExamBindingModel>();


            Mapper.CreateMap<CourseDocument, CourseDocumentViewModel>();
            Mapper.CreateMap<Document, DocumentBindingModel>();
            Mapper.CreateMap<CourseDocumentViewModel, CourseDocument>();
            Mapper.CreateMap<DocumentBindingModel, Document>();

            Mapper.CreateMap<ApplicationSetupViewModel, ApplicationSetups>();
            Mapper.CreateMap<ApplicationSetups, ApplicationSetupViewModel>();

            Mapper.CreateMap<RelevantMembershipViewModel, RelevantMemberships>();
            Mapper.CreateMap<RelevantMemberships, RelevantMembershipViewModel>();

            Mapper.CreateMap<RelevantWorks, RelevantWorkViewModel>();
            Mapper.CreateMap<RelevantWorkViewModel, RelevantWorks>();

            Mapper.CreateMap<Document, RawDocument>();
            Mapper.CreateMap<CourseDocument, RawCourseDocument>();
            Mapper.CreateMap<CourseTran, RawCourseTran>();
            Mapper.CreateMap<Enquiry, RawEnquiry>();
            Mapper.CreateMap<Keyword, RawKeyword>();
            Mapper.CreateMap<ModuleTran, RawModuleTran>();
            Mapper.CreateMap<Course, RawCourseViewModel>();
            Mapper.CreateMap<Module, RawModule>();
            Mapper.CreateMap<ModuleCombination, RawModuleCombination>();
            Mapper.CreateMap<Class, CourseCalendarViewModel>();
            Mapper.CreateMap<Lesson, LessonViewModel>();
            Mapper.CreateMap<CourseCategory, RawCourseCategory>();


            Mapper.CreateMap<ModuleTran, SelectModuleItem>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.ModuleName, m => m.MapFrom(s => s.ModuleName))
                ;
            Mapper.CreateMap<SelectModuleItem, ModuleTran>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.ModuleName, m => m.MapFrom(s => s.ModuleName))
                ;

            Mapper.CreateMap<CmsContent, RawCMSViewModel>();
            Mapper.CreateMap<CmsContentType, RawCmsContentType>();
            Mapper.CreateMap<CmsImage, RawCmsImage>();

            Mapper.CreateMap<NotificationUser, NotificationViewModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.NotificationId))
                .ForMember(d => d.Title, m => m.MapFrom(s => s.Notification.Title))
                .ForMember(d => d.Body, m => m.MapFrom(s => s.Notification.Body))
                .ForMember(d => d.Type, m => m.MapFrom(s => s.Notification.Type))
                .ForMember(d => d.DataId, m => m.MapFrom(s => s.Notification.DataId))
                .ForMember(d => d.IsFavourite, m => m.MapFrom(s => s.IsFavourite))
                .ForMember(d => d.IsRead, m => m.MapFrom(s => s.IsRead))
                .ForMember(d => d.IsRemove, m => m.MapFrom(s => s.IsRemove))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => s.CreatedDate));

            Mapper.CreateMap<AdditionalClassesApproval, AdditionalClassesApprovalViewModel>()
                .ForMember(d=>d.CourseId,m=>m.MapFrom(s=>s.CourseId))
                .ForMember(d=>d.OriginalTargetNumber,m=>m.MapFrom(s=>s.OriginalTargetNumber))
                .ForMember(d=>d.NewTargetNumber,m=>m.MapFrom(s=>s.NewTargetNumber))
                .ForMember(d=>d.AppovalStatusFrom, m=>m.MapFrom(s=>s.StatusFrom))
                .ForMember(d=>d.ApprovalStatusTo, m=>m.MapFrom(s=>s.StatusTo))
                .ForMember(d=>d.ApprovalUpdatedTime, m=>m.MapFrom(s=>s.UpdatedTime))
                .ForMember(d=>d.ApprovalRemarks, m=>m.MapFrom(s=>s.ApprovalRemark))
                .ForMember(d=>d.ListDocuments, m=>m.MapFrom(s=>s.Documents))
                
                
                ;
            Mapper.CreateMap<Document, AdditionalClassesApprovalDocumentViewModel>()
                .ForMember(d => d.FileName, m => m.MapFrom(s => s.FileName))
                .ForMember(d => d.DownloadUrl, m => m.MapFrom(s => ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + s.Id));
        }
    }
}

