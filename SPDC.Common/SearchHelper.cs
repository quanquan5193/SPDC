using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Common
{
    public static class SearchHelper
    {
        public static List<SearchModel> ToElasticSearchList(this List<Course> models, string siteUrl)
        {
            List<SearchModel> dataList = new List<SearchModel>();

            foreach (var course in models)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i <= 3; i++)
                {
                    sb.Clear();
                    sb.Append(course.CourseCode);
                    sb.Append(' ' + course.CourseCategory.CourseCategorieTrans.Single(x => x.LanguageId == i).Name);
                    sb.Append(' ' + course.DurationHrs);
                    sb.Append(' ' + course.Credits);
                    sb.Append(' ' + course.CourseVenue.CourseLocationTrans.Single(x => x.LanguageId == i).Name);
                    sb.Append(' ' + course.CourseFee);
                    sb.Append(' ' + course.DiscountFee);

                    foreach (var module in course.Modules)
                    {
                        sb.Append(' ' + module.ModuleNo + ' ' + module.Fee);
                    }

                    string courseTypeStr = String.Empty;
                    if (i == 1)
                    {
                        courseTypeStr = course.CourseType.NameEN;
                        sb.Append(' ' + course.CourseType.NameEN);
                        if (course.ProgrammeLeader != null)
                            sb.Append(' ' + course.ProgrammeLeader.NameEN);
                        if (course.Lecturer != null)
                            sb.Append(' ' + course.Lecturer.NameEN);
                        if (course.MediumOfInstruction != null)
                            sb.Append(' ' + course.MediumOfInstruction.NameEN);
                    }
                    else if (i == 2)
                    {
                        courseTypeStr = course.CourseType.NameCN;
                        sb.Append(' ' + course.CourseType.NameCN);
                        if (course.ProgrammeLeader != null)
                            sb.Append(' ' + course.ProgrammeLeader.NameCN);
                        if (course.Lecturer != null)
                            sb.Append(' ' + course.Lecturer.NameCN);
                        if (course.MediumOfInstruction != null)
                            sb.Append(' ' + course.MediumOfInstruction.NameCN);
                    }
                    else
                    {
                        courseTypeStr = course.CourseType.NameHK;
                        sb.Append(' ' + course.CourseType.NameHK);
                        if (course.ProgrammeLeader != null)
                            sb.Append(' ' + course.ProgrammeLeader.NameHK);
                        if (course.Lecturer != null)
                            sb.Append(' ' + course.Lecturer.NameHK);
                        if (course.MediumOfInstruction != null)
                            sb.Append(' ' + course.MediumOfInstruction.NameHK);
                    }

                    CourseTran tran = course.CourseTrans.SingleOrDefault(x => x.LanguageId == i);
                    if (tran != null)
                    {
                        sb.Append(' ' + tran.CourseName);
                        sb.Append(' ' + tran.Curriculum);
                        sb.Append(' ' + tran.Recognition);
                        sb.Append(' ' + tran.AdmissionRequirements);
                        sb.Append(' ' + tran.ConditionsOfCertificate);
                    }

                    foreach (var enquiry in course.Enquiries)
                    {
                        if (enquiry != null)
                        {
                            if (i == 1)
                            {
                                sb.Append(' ' + enquiry.ContactPersonEN);
                            }
                            else if (i == 2)
                            {
                                sb.Append(' ' + enquiry.ContactPersonCN);
                            }
                            else
                            {
                                sb.Append(' ' + enquiry.ContactPersonHK);
                            }

                            sb.Append(' ' + enquiry.Email);
                            sb.Append(' ' + enquiry.Phone);
                            sb.Append(' ' + enquiry.Fax);
                        }
                    }

                    SearchModel temp = new SearchModel();
                    temp.Id = $"course-{i}-{course.Id}";
                    temp.DataId = course.Id;
                    temp.DataType = "course";
                    temp.IsVisible = course.InvisibleOnWebsite.HasValue ? (bool)course.InvisibleOnWebsite : false;
                    temp.Description = $"<b>Types of Courses:</b> {course.CourseCategory.CourseCategorieTrans.Single(x => x.LanguageId == i).Name}    <b>Course Code:</b> {course.CourseCode}    <b>Course Fee (HKD):</b> {course.CourseFee}    <b>Duration (Total hrs):</b> {course.DurationTotal}    <b>Study Mode:</b> {courseTypeStr}";
                    temp.Title = course.CourseTrans.SingleOrDefault(x => x.LanguageId == i) == null ? "" : course.CourseTrans.SingleOrDefault(x => x.LanguageId == i).CourseName;
                    temp.Data = sb.ToString();
                    temp.PublishDate = course.LastModifiedDate.HasValue ? (DateTime)course.LastModifiedDate : DateTime.Now;
                    temp.Url = siteUrl + (i == 1 ? "en" : (i == 2 ? "sc" : "tc")) + "/course_search_detail?courseId=" + course.Id;

                    dataList.Add(temp);
                }
            }

            return dataList;
        }

        public static List<SearchModel> ToElasticSearchList(this List<CmsContent> models, string siteUrl)
        {
            List<SearchModel> dataList = new List<SearchModel>();

            foreach (var cms in models)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(cms.Title);
                sb.Append(' ' + cms.ShortDescription);
                sb.Append(' ' + cms.FullDescription);
                sb.Append(' ' + cms.CreateDate.ToString("dd/MM/yyyy hh:mm tt"));

                SearchModel temp = new SearchModel();
                temp.Id = $"cms-0-{cms.Id}";
                temp.DataId = cms.Id;
                temp.DataType = "cms";
                temp.IsVisible = cms.PublishStatus && cms.ShowOnLandingPage;
                temp.Description = "";
                temp.Title = cms.Title;
                temp.Data = sb.ToString();
                temp.PublishDate = cms.LastPublishDate.HasValue ? (DateTime)cms.LastPublishDate : DateTime.Now;

                temp.Url = siteUrl + CmsUrlhelper(cms.ContentTypeId, true) + "/" + CmsUrlhelper(cms.ContentTypeId, false) + "/" + cms.Id + "/" + cms.SEOUrlLink;

                dataList.Add(temp);
            }
            return dataList;
        }

        public static List<SearchModel> ToElasticSearchList(this List<CourseDocument> models, string siteUrl)
        {
            List<SearchModel> dataList = new List<SearchModel>();
            string[] fileExs = new string[] { ".pptx", ".ppt", ".doc", ".docx", ".xls", ".xlsx", ".pdf" };

            foreach (var doc in models)
            {
                StringBuilder sb = new StringBuilder();

                SearchModel temp = new SearchModel();
                temp.Id = $"document-0-{doc.Id}";
                temp.DataId = doc.Document.Id;
                temp.DataType = "document";
                temp.IsVisible = doc.Course.InvisibleOnWebsite.HasValue ? (bool)doc.Course.InvisibleOnWebsite : false;
                temp.Description = "";
                temp.Title = doc.Document.FileName;
                temp.Data = "";
                temp.PublishDate = doc.Document.ModifiedDate;
                temp.Url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + doc.Document.Id;

                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var pathDoc = Path.Combine(serPath, doc.Document.Url);

                if (File.Exists(pathDoc))
                {
                    if (fileExs.Contains(doc.Document.FileName.Substring(doc.Document.FileName.LastIndexOf("."))))
                    {
                        var base64File = Convert.ToBase64String(File.ReadAllBytes(pathDoc));
                        temp.Path = pathDoc;
                        temp.Content = base64File;
                        dataList.Add(temp);
                    }
                }
            }
            return dataList;
        }

        public static string CmsUrlhelper(int cmsType, bool isMultiLang)
        {
            int[] cmsTypeENIds = new int[] { 6, 9, 12, 15, 18, 21 };
            int[] cmsTypeTCIds = new int[] { 7, 10, 13, 16, 19, 22 };
            int[] cmsTypeSCIds = new int[] { 8, 11, 14, 17, 20, 23 };
            int[] cmsTypeAnnouncementIds = new int[] { 6, 7, 8 };
            int[] cmsTypeNewsAndEventsIds = new int[] { 9, 10, 11, 12, 13, 14 };
            int[] cmsTypeHotCourseIds = new int[] { 15, 16, 17 };
            int[] cmsTypeSTEMIds = new int[] { 18, 19, 20 };
            int[] cmsTypeCareerProgessionIds = new int[] { 21, 22, 23 };

            if (isMultiLang)
            {
                if (cmsTypeENIds.Contains(cmsType))
                {
                    return "en";
                }
                if (cmsTypeSCIds.Contains(cmsType))
                {
                    return "sc";
                }
                if (cmsTypeTCIds.Contains(cmsType))
                {
                    return "tc";
                }
            }
            else
            {
                if (cmsTypeAnnouncementIds.Contains(cmsType))
                {
                    return "announcements";
                }
                if (cmsTypeNewsAndEventsIds.Contains(cmsType))
                {
                    return "new-events";
                }
                if (cmsTypeHotCourseIds.Contains(cmsType))
                {
                    return "hot-courses";
                }
                if (cmsTypeSTEMIds.Contains(cmsType))
                {
                    return "stem-alliances";
                }
                if (cmsTypeCareerProgessionIds.Contains(cmsType))
                {
                    return "progression-advancements";
                }
            }

            return "";
        }
    }
}
