using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using SPDC.Model.Models;

namespace SPDC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.UseDatabaseNullSemantics = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Application> Applications { get; set; }

        public virtual DbSet<ApplicationStatusStorage> ApplicationStatusStorages { get; set; }

        public virtual DbSet<ApplicationTran> ApplicationTrans { get; set; }

        public virtual DbSet<Class> Classes { get; set; }

        public virtual DbSet<CourseCategory> CourseCategories { get; set; }

        public virtual DbSet<CourseCategorieTran> CourseCategorieTrans { get; set; }

        public virtual DbSet<CourseDocument> CourseDocuments { get; set; }

        public virtual DbSet<CourseLocation> CourseLocations { get; set; }

        public virtual DbSet<CourseLocationTran> CourseLocationTrans { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<CourseTran> CourseTrans { get; set; }

        public virtual DbSet<CourseType> CourseTypes { get; set; }

        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<DownloadDocumentTracking> DownloadDocumentTrackings { get; set; }

        public virtual DbSet<EmployerRecommendation> EmployerRecommendations { get; set; }

        public virtual DbSet<EmployerRecommendationTran> EmployerRecommendationTrans { get; set; }

        public virtual DbSet<Enquiry> EnquiryEmails { get; set; }

        public virtual DbSet<EnrollmentStatus> EnrollmentStatus { get; set; }

        public virtual DbSet<EnrollmentStatusStorage> EnrollmentStatusStorages { get; set; }

        public virtual DbSet<Exam> Exams { get; set; }

        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

        public virtual DbSet<InvoiceItemTran> InvoiceItemTrans { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<InvoiceStatus> InvoiceStatus { get; set; }

        public virtual DbSet<InvoiceStatusStorage> InvoiceStatusStorages { get; set; }

        public virtual DbSet<InvoiceItemType> InvoiceItemTypes { get; set; }

        public virtual DbSet<Keyword> Keywords { get; set; }

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<LessonAttendance> LessonAttendances { get; set; }

        public virtual DbSet<Lesson> Lessons { get; set; }

        public virtual DbSet<Module> Modules { get; set; }

        public virtual DbSet<ModuleTran> ModuleTrans { get; set; }

        public virtual DbSet<Particular> Particulars { get; set; }

        public virtual DbSet<ParticularTran> ParticularTrans { get; set; }

        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public virtual DbSet<Qualification> Qualifications { get; set; }

        public virtual DbSet<QualificationsTran> QualificationsTrans { get; set; }

        public virtual DbSet<StudentRemark> StudentRemarks { get; set; }

        public virtual DbSet<SystemPrivilege> SystemPrivileges { get; set; }

        public virtual DbSet<TransactionType> TransactionTypes { get; set; }

        public virtual DbSet<UserDocument> UserDocuments { get; set; }

        public virtual DbSet<WorkExperience> WorkExperiences { get; set; }

        public virtual DbSet<WorkExperienceTran> WorkExperienceTrans { get; set; }

        public virtual DbSet<ModuleCombination> ModuleCombinations { get; set; }

        public virtual DbSet<CmsContent> CmsContents { get; set; }

        public virtual DbSet<CmsImage> CmsImages { get; set; }

        public virtual DbSet<CmsContentType> CmsContentTypes { get; set; }

        public virtual DbSet<ClassCommon> ClassCommon { get; set; }

        public virtual DbSet<Lecturer> Lecturers { get; set; }

        public virtual DbSet<ProgrammeLeader> ProgrammeLeaders { get; set; }

        public virtual DbSet<LevelofApproval> LevelofApprovals { get; set; }

        public virtual DbSet<MediumOfInstruction> MediumOfInstructions { get; set; }

        public virtual DbSet<ApplicationSetups> ApplicationSetups { get; set; }

        public virtual DbSet<RelevantWorks> RelevantWorks { get; set; }

        public virtual DbSet<RelevantMemberships> RelevantMemberships { get; set; }

        public virtual DbSet<ClientApplication> ClientApplications { get; set; }

        public virtual DbSet<TargetClasses> TargetClasses { get; set; }

        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

        public virtual DbSet<CourseAppovedStatusHistory> CourseAppovedStatusHistories { get; set; }

        public virtual DbSet<ClassAppovedStatusHistory> ClassAppovedStatusHistories { get; set; }

        public virtual DbSet<CourseHistoryDocument> CourseHistoryDocuments { get; set; }

        public virtual DbSet<ClassHistoryDocument> ClassHistoryDocuments { get; set; }

        public virtual DbSet<ApplicationApprovedStatusHistory> ApplicationApprovedStatusHistories { get; set; }

        public virtual DbSet<UserDevice> UserDevices { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<NotificationUser> NotificationUsers { get; set; }

        public virtual DbSet<AcceptedBank> AcceptedBanks { get; set; }

        public virtual DbSet<AdditionalClassesApproval> AdditionalClassesApprovals { get; set; }

        public virtual DbSet<RefundTransaction> RefundTransactions { get; set; }

        public virtual DbSet<RefundTransactionApprovedStatusHistory> RefundTransactionApprovedStatusHistories { get; set; }

        public virtual DbSet<RefundTransactionDocument> RefundTransactionDocuments { get; set; }

        public virtual DbSet<RefundTransactionHistoryDocument> RefundTransactionHistoryDocuments { get; set; }

        public virtual DbSet<BatchPayment> BatchPayments { get; set; }

        public virtual DbSet<ParticularTemp> ParticularTemps { get; set; }

        public virtual DbSet<ParticularTempTran> ParticularTempTrans { get; set; }

        public virtual DbSet<QualificationTemp> QualificationTemps { get; set; }

        public virtual DbSet<QualificationTempTran> QualificationTempTrans { get; set; }

        public virtual DbSet<WorkExperienceTemp> WorkExperienceTemps { get; set; }

        public virtual DbSet<WorkExperienceTempTran> WorkExperienceTempTrans { get; set; }

        public virtual DbSet<EmployerRecommendationTemp> EmployerRecommendationTemps { get; set; }

        public virtual DbSet<EmployerRecommendationTempTran> EmployerRecommendationTempTrans { get; set; }

        public virtual DbSet<MakeUpAttendence> MakeUpAttendences { get; set; }

        public virtual DbSet<MakeUpClass> MakeUpClasses { get; set; }

        public virtual DbSet<ResitExam> ResitExams { get; set; }

        public virtual DbSet<ResitExamApplication> ResitExamApplications { get; set; }
        
        public virtual DbSet<ApplicationAssessmentDocument> ApplicationAssessmentDocuments { get; set; }

        public virtual DbSet<VersionManagement> VersionManagements { get; set; }

        public virtual DbSet<CommonData> CommonData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomUserLogin>().ToTable("UserLogins").HasKey(x => new { x.UserId, x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<CustomUserClaim>().ToTable("UserClaims").HasKey(x => x.Id);
            modelBuilder.Entity<CustomUserRole>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasKey(x => x.Id);
            modelBuilder.Entity<CustomRole>().ToTable("Roles").HasKey(x => x.Id);

            modelBuilder.Entity<Application>()
                .Property(e => e.ApplicationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.ApplicationStatusStorages)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.ApplicationTrans)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.EnrollmentStatusStorages)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.LessonAttendances)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Application>()
            //    .HasMany(e => e.PaymentTransactions)
            //    .WithRequired(e => e.Application)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.PaymentTransactions)
                .WithRequired(e => e.Invoice)
                .HasForeignKey(e => e.InvoiceId);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.DownloadDocumentTrackings)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.ClassCode)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.ReExamFees)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.StudentPreferredApplicationModels)
                .WithOptional(e => e.StudentPreferredClassModel)
                .HasForeignKey(e => e.StudentPreferredClass);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.AdminAssignedApplicationModels)
                .WithOptional(e => e.AdminAssignedClassModel)
                .HasForeignKey(e => e.AdminAssignedClass);

            

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Exams)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Lessons)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseCategory>()
                .HasMany(e => e.CourseCategories1)
                .WithOptional(e => e.CourseCategory1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<CourseCategory>()
                .HasMany(e => e.CourseCategorieTrans)
                .WithRequired(e => e.CourseCategory)
                .HasForeignKey(e => e.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseCategory>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.CourseCategory)
                .HasForeignKey(e => e.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseDocument>()
                .HasMany(e => e.DownloadDocumentTrackings)
                .WithRequired(e => e.CourseDocument)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseLocation>()
                .HasMany(e => e.CourseLocationTrans)
                .WithRequired(e => e.CourseLocation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseLocation>()
                .HasMany(e => e.Lessons)
                .WithRequired(e => e.CourseLocation)
                .HasForeignKey(e => e.LocationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseCode)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseFee)
                .HasPrecision(17, 1);

            modelBuilder.Entity<Course>()
                .Property(e => e.Allowance)
                .HasPrecision(17, 1);

            modelBuilder.Entity<Course>()
                .Property(e => e.DiscountFee)
                .HasPrecision(17, 1);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Classes)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseDocuments)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseTrans)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Enquiries)
                .WithOptional(e => e.Course)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.ModuleCombinations)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Keywords)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.StudentRemarkModels)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.SystemPrivileges)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseType>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.CourseType)
                .HasForeignKey(e => e.CourseTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.ContentType)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.CourseDocuments)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.UserDocuments)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployerRecommendation>()
                .Property(e => e.Tel)
                .IsUnicode(false);

            modelBuilder.Entity<EmployerRecommendation>()
                .HasMany(e => e.EmployerRecommendationTrans)
                .WithRequired(e => e.EmployerRecommendation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Enquiry>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<EnrollmentStatus>()
                .HasMany(e => e.EnrollmentStatusStorages)
                .WithRequired(e => e.EnrollmentStatus)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Exam>()
                .Property(e => e.FromTime)
                .IsUnicode(false);

            modelBuilder.Entity<Exam>()
                .Property(e => e.ToTime)
                .IsUnicode(false);

            modelBuilder.Entity<Exam>()
                .Property(e => e.Marks)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceItem>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<InvoiceItem>()
                .HasMany(e => e.InvoiceItemTrans)
                .WithRequired(e => e.InvoiceItem)
                .HasForeignKey(e => e.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.InvoiceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.Fee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceItems)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceStatusStorages)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceStatus>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.InvoiceStatus)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceStatus>()
                .HasMany(e => e.InvoiceStatusStorages)
                .WithRequired(e => e.InvoiceStatus)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Keyword>()
                .Property(e => e.WordEN)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.ApplicationTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.CourseCategorieTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.CourseLocationTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.CourseTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.EmployerRecommendationTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.InvoiceItemTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.ParticularTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.QualificationsTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Language)
                .HasForeignKey(e => e.CommunicationLanguage);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.WorkExperienceTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.ModuleTrans)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Lesson>()
            //    .Property(e => e.FromTime)
            //    .IsUnicode(false);

            modelBuilder.Entity<Lesson>()
                .HasMany(e => e.LessonAttendances)
                .WithRequired(e => e.Lesson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Fee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.ModuleTrans)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.SurnameEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.GivenNameEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.MobileNumberPrefix)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.TelNo)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.FaxNo)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.RegionEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.DistrictEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.StreetNumberEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.StreetRoadEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.EstateQuartersVillageEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.BuildingEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.FloorEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.RmFtUnitEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.EducationLevelEN)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .Property(e => e.RelatedQualifications2Year)
                .IsUnicode(false);

            modelBuilder.Entity<Particular>()
                .HasMany(e => e.ParticularTrans)
                .WithRequired(e => e.Particular)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(e => e.Particular)
                .WithRequired(e => e.User);

            modelBuilder.Entity<ParticularTemp>()
                .HasMany(e => e.ParticularTempTrans)
                .WithRequired(e => e.ParticularTemp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Qualifications)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QualificationTemp>()
                .HasMany(e => e.QualificationTempTrans)
                .WithRequired(e => e.QualificationTemp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.WorkExperienceTemps)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkExperienceTemp>()
                .HasMany(e => e.WorkExperienceTempTrans)
                .WithRequired(e => e.WorkExperienceTemp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.EmployerRecommendations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployerRecommendationTemp>()
                .HasMany(e => e.EmployerRecommendationTempTrans)
                .WithRequired(e => e.EmployerRecommendationTemp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentTransaction>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PaymentTransaction>()
                .Property(e => e.RefNo)
                .IsUnicode(false);

            modelBuilder.Entity<Qualification>()
                .HasMany(e => e.QualificationsTrans)
                .WithRequired(e => e.Qualification)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.PaymentTransactions)
                .WithRequired(e => e.TransactionType1)
                .HasForeignKey(e => e.TransactionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.CICNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.OtherEmail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.EmployerRecommendations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.LessonAttendances)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(e => e.Particular)
                .WithRequired(e => e.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(e => e.AdminPermission)
                .WithRequired(e => e.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Qualifications)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.SystemPrivileges)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UserDocuments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UserDocumentTemps)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.WorkExperiences)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UserDevices)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.NotificationUsers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>()
                .HasMany(e => e.NotificationUsers)
                .WithRequired(e => e.Notification)
                .HasForeignKey(e => e.NotificationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkExperience>()
                .HasMany(e => e.WorkExperienceTrans)
                .WithRequired(e => e.WorkExperience)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CmsContentType>()
                .HasMany(e => e.CmsContents)
                .WithRequired(e => e.CmsContentType)
                .HasForeignKey(e => e.ContentTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CmsContent>()
                .HasMany(e => e.CmsImages)
                .WithOptional(e => e.CmsContent)
                .HasForeignKey(e => e.CmsId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(e => e.AdminPermission)
                .WithRequired(e => e.User);
            modelBuilder.Entity<Lecturer>()
               .HasMany(e => e.Courses)
               .WithOptional(e => e.Lecturer)
               .HasForeignKey(e => e.LecturerId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProgrammeLeader>()
               .HasMany(e => e.Courses)
               .WithOptional(e => e.ProgrammeLeader)
               .HasForeignKey(e => e.ProgrammeLeaderId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<LevelofApproval>()
               .HasMany(e => e.Courses)
               .WithOptional(e => e.LevelofApproval)
               .HasForeignKey(e => e.LevelOfApprovalId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<MediumOfInstruction>()
                .HasMany(e => e.Courses)
                .WithOptional(e => e.MediumOfInstruction)
                .HasForeignKey(e => e.MediumOfInstructionId);

            modelBuilder.Entity<CourseLocation>()
                .HasMany(e => e.Courses)
                .WithOptional(e => e.CourseVenue)
                .HasForeignKey(e => e.CourseVenueId);

            modelBuilder.Entity<Course>()
                .HasOptional(e => e.ApplicationSetup)
                .WithRequired(e => e.Course)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationSetups>()
                .HasOptional(e => e.RelevantMembership)
                .WithRequired(e => e.ApplicationSetup);

            modelBuilder.Entity<ApplicationSetups>()
                .HasOptional(e => e.RelevantWork)
                .WithRequired(e => e.ApplicationSetup);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.CourseAppovedStatusHistories)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseAppovedStatusHistory>()
                .HasMany(e => e.CourseHistoryDocuments)
                .WithRequired(e => e.CourseAppovedStatusHistory)
                .HasForeignKey(e => e.CourseHistoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.CourseHistoryDocuments)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.ClassAppovedStatusHistories)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClassAppovedStatusHistory>()
                .HasMany(e => e.ClassHistoryDocuments)
                .WithRequired(e => e.ClassAppovedStatusHistory)
                .HasForeignKey(e => e.ClassHistoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.ClassHistoryDocuments)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.SubClassApprovedStatusHistories)
                .WithRequired(e => e.Class)
                .HasForeignKey(e => e.ClassId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.AdditionalClassesApprovals)
                .WithRequired(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AdditionalClassesApproval>()
            .HasMany(s => s.Documents)
            .WithMany(c => c.AdditionalClassesApprovals)
            .Map(cs =>
            {
                cs.MapLeftKey("AdditionalClassesApprovalRefId");
                cs.MapRightKey("DocumentRefId");
                cs.ToTable("AdditionalClassesApprovalDocument");
            });

            modelBuilder.Entity<SubClassApprovedStatusHistory>()
                .HasMany(e => e.SubClassHistoryDocuments)
                .WithRequired(e => e.SubClassApprovedStatusHistory)
                .HasForeignKey(e => e.SubClassHistoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.SubClassHistoryDocuments)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.DocumentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClassCommon>()
                .HasMany(e => e.AttendanceRequirementTypes)
                .WithOptional(e => e.ClassCommon)
                .HasForeignKey(e => e.ClassCommonId);

            modelBuilder.Entity<ClassCommon>()
                .HasMany(e => e.NewAttendanceRequirementTypes)
                .WithOptional(e => e.NewAttendanceRequirementType)
                .HasForeignKey(e => e.NewAttendanceRequirementTypeId);

            modelBuilder.Entity<SubClassDraft>()
                .HasMany(e => e.SubClassApprovedStatusHistories)
                .WithRequired(e => e.SubClassDraft)
                .HasForeignKey(e => e.SubClassDraftId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.SubClassDrafts)
                .WithRequired(e => e.Class)
                .HasForeignKey(e => e.ClassId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.ApplicationApprovedStatusHistories)
                .WithRequired(e => e.Application)
                .HasForeignKey(e => e.ApplicationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationApprovedStatusHistory>()
                .HasMany(e => e.ApplicationHistoryDocuments)
                .WithRequired(e => e.ApplicationApprovedStatusHistory)
                .HasForeignKey(e => e.ApplicationHistoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.ApplicationHistoryDocuments)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.DocumentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceItemType>()
                .HasMany(e => e.InvoiceItems)
                .WithRequired(e => e.InvoiceItemType)
                .HasForeignKey(e => e.InvoiceItemTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.WaiverApprovedStatusHistories)
                .WithRequired(e => e.Invoice)
                .HasForeignKey(e => e.InvoiceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WaiverApprovedStatusHistory>()
                .HasMany(e => e.WaivedHistoryDocuments)
                .WithRequired(e => e.WaiverApprovedStatusHistory)
                .HasForeignKey(e => e.WaiverApprovedStatusHistoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.WaivedHistoryDocuments)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.WaiverApprovedStatusHistoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AcceptedBank>()
                .HasMany(e => e.PaymentTransactions)
                .WithRequired(e => e.AcceptedBank)
                .HasForeignKey(e => e.AcceptedBankId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentTransaction>()
                .HasMany(e => e.PaymentTransactionDocuments)
                .WithRequired(e => e.PaymentTransaction)
                .HasForeignKey(e => e.PaymentTransactionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.PaymentTransactionDocuments)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.DocumentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RefundTransaction>()
                .HasMany(e => e.RefundTransactionDocuments)
                .WithRequired(e => e.RefundTransaction)
                .HasForeignKey(e => e.RefundTransactionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.RefundTransactionDocuments)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.DocumentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.RefundTransactions)
                .WithRequired(e => e.TransactionType)
                .HasForeignKey(e => e.TransactionTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AcceptedBank>()
                .HasMany(e => e.RefundTransactions)
                .WithRequired(e => e.AcceptedBank)
                .HasForeignKey(e => e.AcceptedBankId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.RefundTransactionHistoryDocuments)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.DocumentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RefundTransactionApprovedStatusHistory>()
            .HasMany(e => e.RefundTransactionHistoryDocuments)
            .WithRequired(e => e.RefundTransactionApprovedStatusHistory)
            .HasForeignKey(e => e.RefundTransactionApprovedStatusHistoryId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.RefundTransactions)
                .WithRequired(e => e.Invoice)
                .HasForeignKey(e => e.InvoiceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RefundTransaction>()
                .HasMany(e => e.RefundTransactionApprovedStatusHistories)
                .WithRequired(e => e.RefundTransaction)
                .HasForeignKey(e => e.RefundTransactionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.BatchPayments)
                .WithRequired(e => e.TransactionType)
                .HasForeignKey(e => e.TransactionTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AcceptedBank>()
                .HasMany(e => e.BatchPayments)
                .WithRequired(e => e.AcceptedBank)
                .HasForeignKey(e => e.AcceptedBankId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BatchPayment>()
            .HasMany(s => s.Documents)
            .WithMany(c => c.BatchPayments)
            .Map(cs =>
            {
                cs.MapLeftKey("BatchPaymentId");
                cs.MapRightKey("DocumentId");
                cs.ToTable("BatchPaymentDocument");
            });

            modelBuilder.Entity<TargetClasses>()
                .Property(e => e.AcademicYear)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasOptional(e => e.TargetClasses)
                .WithRequired(e => e.Course);

            // MakeUpClass
            modelBuilder.Entity<Document>()
                .HasMany(e => e.MakeUpClasses)
                .WithMany(e => e.Documents)
                .Map(cs =>
                {
                    cs.MapLeftKey("MakeUpClassRefId");
                    cs.MapRightKey("DocumentRefId");
                    cs.ToTable("MakeUpClassDocument");
                });

            modelBuilder.Entity<Document>()
                .HasMany(e => e.ApplicationAttendanceDocuments)
                .WithMany(e => e.ApplicationAttendanceDocuments)
                .Map(cs =>
                {
                    cs.MapLeftKey("DocumentRefId");
                    cs.MapRightKey("ApplicationRefId");
                    cs.ToTable("ApplicationAttendanceDocument");
                });

            modelBuilder.Entity<MakeUpClass>()
                .HasMany(e => e.MakeUpAttendences)
                .WithRequired(e => e.MakeUpClass)
                .HasForeignKey(e => e.MakeUpClassId)
                .WillCascadeOnDelete(false); 

            modelBuilder.Entity<Application>()
                .HasMany(e => e.MakeUpAttendences)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lesson>()
                .HasMany(e => e.MakeUpAttendences)
                .WithRequired(e => e.Lesson)
                .WillCascadeOnDelete(false);

            // Exam
            modelBuilder.Entity<Application>()
                .HasMany(e => e.ExamApplications)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResitExam>()
                .HasMany(e => e.ResitExamApplications)
                .WithRequired(e => e.ResitExam)
                .HasForeignKey(e => e.ResitExamId);

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.ResitExamApplications)
                .WithRequired(e => e.Exam)
                .HasForeignKey(e=>e.Exam_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.ResitExamApplications)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.ApplicationAssessmentDocuments)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Document>()
            //    .HasMany(e => e.ApplicationAttendanceDocuments)
            //    .WithRequired(e => e.Document)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.ApplicationAssessmentDocuments)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Application>()
            //    .HasMany(e => e.ApplicationAttendanceDocuments)
            //    .WithRequired(e => e.Application)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.ExamApplications)
                .WithRequired(e => e.Exam)
                .HasForeignKey(e => e.ExamId);

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.FromExams)
                .WithOptional(e => e.FromExam)
                .HasForeignKey(e => e.FromExamId);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.UserDocumentsTemps)
                .WithRequired(e => e.Document)
                .WillCascadeOnDelete(false);
        }
    }
}
