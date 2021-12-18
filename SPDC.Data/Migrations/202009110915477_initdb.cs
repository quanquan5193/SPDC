namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        EnrollmentStatus = c.Int(nullable: false),
                        StudentPreferredClass = c.Int(),
                        AdminAssignedClass = c.Int(),
                        IsAvailable = c.Boolean(),
                        ApplicationNumber = c.String(maxLength: 20, unicode: false),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.AdminAssignedClass)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Classes", t => t.StudentPreferredClass)
                .ForeignKey("dbo.ApplicationStatus", t => t.Status)
                .ForeignKey("dbo.EnrollmentStatus", t => t.EnrollmentStatus)
                .Index(t => t.CourseId)
                .Index(t => t.UserId)
                .Index(t => t.Status)
                .Index(t => t.EnrollmentStatus)
                .Index(t => t.StudentPreferredClass)
                .Index(t => t.AdminAssignedClass)
                .Index(t => t.ApplicationNumber, unique: true, name: "INDEX_APPNUM");
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassCode = c.String(nullable: false, maxLength: 20, unicode: false),
                        CourseId = c.Int(nullable: false),
                        CommencementDate = c.DateTime(nullable: false),
                        CompletionDate = c.DateTime(nullable: false),
                        IsExam = c.Boolean(nullable: false),
                        IsReExam = c.Boolean(nullable: false),
                        ExamPassingMask = c.Int(),
                        ReExamFees = c.Decimal(precision: 18, scale: 0),
                        AcademicYear = c.String(maxLength: 256, unicode: false),
                        InvisibleOnWebsite = c.Boolean(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.ClassCode, unique: true, name: "INDEX_CLASSCODE")
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(nullable: false, maxLength: 20, unicode: false),
                        CategoryId = c.Int(nullable: false),
                        CourseTypeId = c.Int(nullable: false),
                        CourseFee = c.Decimal(nullable: false, precision: 18, scale: 0),
                        FeeRemarks = c.String(maxLength: 256),
                        Allowance = c.Decimal(precision: 18, scale: 0),
                        IsAllowanceProvided = c.Boolean(nullable: false),
                        DiscountFee = c.Decimal(precision: 18, scale: 0),
                        CommencementDate = c.DateTime(nullable: false, storeType: "date"),
                        CompletionDate = c.DateTime(nullable: false, storeType: "date"),
                        ProgrammeLeaderId = c.Int(nullable: false),
                        MediumOfInstruction = c.Int(nullable: false),
                        ByModule = c.Boolean(nullable: false),
                        EnquiriesNumber = c.String(maxLength: 256, unicode: false),
                        IsShowEducationLevel = c.Boolean(nullable: false),
                        IsShowMemvershipsQualifications = c.Boolean(nullable: false),
                        IsShowWorkExperiences = c.Boolean(nullable: false),
                        IsShowEmployerRecommendation = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatedBy = c.Int(),
                        DateModified = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Int(nullable: false),
                        InvisibleOnWebsite = c.Boolean(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        CourseVenueId = c.Int(nullable: false),
                        DurationHrs = c.Single(nullable: false),
                        DurationLesson = c.Int(nullable: false),
                        TargetClassSize = c.Int(nullable: false),
                        Credits = c.Single(nullable: false),
                        LevelOfApproval = c.String(maxLength: 256),
                        ApplicationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.CourseTypes", t => t.CourseTypeId)
                .ForeignKey("dbo.CourseCategories", t => t.CategoryId)
                .Index(t => t.CourseCode, unique: true, name: "INDEX_COURSECODE")
                .Index(t => t.CategoryId)
                .Index(t => t.CourseTypeId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.CourseCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseCategories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.CourseCategorieTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Title = c.String(maxLength: 256),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.CourseCategories", t => t.CategoryId)
                .Index(t => t.LanguageId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Code = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        Others = c.String(maxLength: 256),
                        Remarks = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.LanguageId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.CourseLocationTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        CourseLocationId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Desciption = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseLocation", t => t.CourseLocationId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.CourseLocationId);
            
            CreateTable(
                "dbo.CourseLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        ExamVenue = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        FromTime = c.String(nullable: false, maxLength: 256, unicode: false),
                        ToTime = c.String(nullable: false, maxLength: 256, unicode: false),
                        Dateline = c.DateTime(nullable: false, storeType: "date"),
                        Marks = c.String(maxLength: 256, unicode: false),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseLocation", t => t.ExamVenue)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.ClassId)
                .Index(t => t.ExamVenue);
            
            CreateTable(
                "dbo.ExamTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        ExamId = c.Int(nullable: false),
                        Result = c.String(maxLength: 256),
                        Remarks = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.ExamId);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        No = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        FromTime = c.String(nullable: false, maxLength: 256, unicode: false),
                        ToTime = c.String(nullable: false, maxLength: 256),
                        LocationId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseLocation", t => t.LocationId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.ClassId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.CourseDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                        LessonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .ForeignKey("dbo.Lessons", t => t.LessonId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId)
                .Index(t => t.DocumentId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 256, unicode: false),
                        ContentType = c.String(nullable: false, maxLength: 256, unicode: false),
                        FileName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.UserId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CICNumber = c.String(maxLength: 50, unicode: false),
                        Status = c.Int(),
                        CommunicationLanguage = c.Int(),
                        IsNotReceiveInfomation = c.Boolean(nullable: false),
                        OtherEmail = c.String(maxLength: 256, unicode: false),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DisplayName = c.String(maxLength: 256),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.CommunicationLanguage)
                .Index(t => t.CICNumber, unique: true, name: "INDEX_CICNUMBER")
                .Index(t => t.CommunicationLanguage)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmployerRecommendations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tel = c.String(nullable: false, maxLength: 256, unicode: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmployerRecommendationTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 256),
                        ContactPerson = c.String(nullable: false, maxLength: 256),
                        LanguageId = c.Int(nullable: false),
                        EmployerRecommendationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployerRecommendations", t => t.EmployerRecommendationId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.EmployerRecommendationId);
            
            CreateTable(
                "dbo.Lecturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LessonAttendances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsTakeAttendance = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Lessons", t => t.LessonId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.LessonId)
                .Index(t => t.ApplicationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Particular",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SurnameEN = c.String(nullable: false, maxLength: 256, unicode: false),
                        GivenNameEN = c.String(nullable: false, maxLength: 256, unicode: false),
                        SurnameCN = c.String(maxLength: 256),
                        GivenNameCN = c.String(maxLength: 256),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Boolean(),
                        HKIDNo = c.Binary(maxLength: 66),
                        PassportNo = c.Binary(maxLength: 66),
                        PassportExpiryDate = c.DateTime(storeType: "date"),
                        MobileNumber = c.Binary(maxLength: 66),
                        MobileNumberPrefix = c.String(maxLength: 256, unicode: false),
                        TelNo = c.String(maxLength: 256, unicode: false),
                        FaxNo = c.String(maxLength: 256, unicode: false),
                        RegionEN = c.String(maxLength: 256, unicode: false),
                        RegionCN = c.String(maxLength: 256),
                        DistrictEN = c.String(maxLength: 256, unicode: false),
                        DistrictCN = c.String(maxLength: 256),
                        StreetNumberEN = c.String(maxLength: 256, unicode: false),
                        StreetNumberCN = c.String(maxLength: 256),
                        StreetRoadEN = c.String(maxLength: 256, unicode: false),
                        StreetRoadCN = c.String(maxLength: 256),
                        EstateQuartersVillageEN = c.String(maxLength: 256, unicode: false),
                        EstateQuartersVillageCN = c.String(maxLength: 256),
                        BuildingEN = c.String(maxLength: 256, unicode: false),
                        BuildingCN = c.String(maxLength: 256),
                        FloorEN = c.String(maxLength: 256, unicode: false),
                        FloorCN = c.String(maxLength: 256),
                        RmFtUnitEN = c.String(maxLength: 256, unicode: false),
                        RmFtUnitCN = c.String(maxLength: 256),
                        EducationLevelEN = c.String(maxLength: 256, unicode: false),
                        EducationLevelCN = c.String(maxLength: 256),
                        RelatedQualifications1Check = c.Boolean(),
                        RelatedQualifications2Check = c.Boolean(),
                        RelatedQualifications2Year = c.String(maxLength: 256, unicode: false),
                        RelatedQualifications3Check = c.Boolean(),
                        Honorific = c.Int(nullable: false),
                        HKIDNoEncrypted = c.String(),
                        PassportNoEncrypted = c.String(),
                        MobileNumberEncrypted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.MobileNumber, unique: true, name: "INDEX_MOBILENUM");
            
            CreateTable(
                "dbo.ParticularTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParticularId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        RelatedQualifications1Text = c.String(maxLength: 256),
                        RelatedQualifications2Text = c.String(maxLength: 256),
                        PresentEmployer = c.String(maxLength: 256),
                        Position = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Particular", t => t.ParticularId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.ParticularId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.PaymentTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 0),
                        PaymentDate = c.DateTime(nullable: false),
                        RefNo = c.String(nullable: false, maxLength: 265, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionType)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.TransactionType)
                .Index(t => t.UserId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.PaymentTransactionTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        PaymentTransactionId = c.Int(nullable: false),
                        BankName = c.String(nullable: false, maxLength: 256),
                        Remarks = c.String(nullable: false, maxLength: 256),
                        ReasonForRefund = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentTransactions", t => t.PaymentTransactionId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.PaymentTransactionId);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateObtained = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.QualificationsTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssuingAuthority = c.String(nullable: false, maxLength: 256),
                        LevelAttained = c.String(nullable: false, maxLength: 256),
                        QualificationId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Qualifications", t => t.QualificationId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.QualificationId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SystemPrivileges",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        FirstApproved = c.Boolean(nullable: false),
                        SecondApproved = c.Boolean(nullable: false),
                        Other = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CourseId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.WorkExperiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FromYear = c.String(nullable: false, maxLength: 256, unicode: false),
                        ToYear = c.String(nullable: false, maxLength: 256, unicode: false),
                        BIMRelated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WorkExperienceTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        WorkExperienceId = c.Int(nullable: false),
                        Location = c.String(nullable: false, maxLength: 256),
                        Position = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkExperiences", t => t.WorkExperienceId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.WorkExperienceId);
            
            CreateTable(
                "dbo.DownloadDocumentTrackings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseDocumentId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        DownloadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseDocuments", t => t.CourseDocumentId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.CourseDocumentId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.CourseTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        CourseName = c.String(nullable: false, maxLength: 256),
                        CourseTitle = c.String(nullable: false, maxLength: 256),
                        Curriculum = c.String(maxLength: 256),
                        ConditionsOfCertificate = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.LanguageId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.InvoiceItemTrans",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        ItemName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.InvoiceItems", t => t.ItemId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.ItemId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        InvoiceType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        InvoiceNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 0),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoiceStatus", t => t.Status)
                .ForeignKey("dbo.InvoiceTypes", t => t.InvoiceType)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.InvoiceType)
                .Index(t => t.Status)
                .Index(t => t.InvoiceNumber, unique: true, name: "INDEX_INVOICENUM");
            
            CreateTable(
                "dbo.InvoiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceStatusStorage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LastModifiedBy = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoiceStatus", t => t.Status)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId)
                .Index(t => t.Status);
            
            CreateTable(
                "dbo.InvoiceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.LocalizationLabels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Key = c.String(nullable: false, maxLength: 256, unicode: false),
                        Value = c.String(maxLength: 256),
                        FormName = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.ModuleTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        ModuleName = c.String(maxLength: 256),
                        Hours = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.LanguageId)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 0),
                        ModuleNo = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.EnquiryEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(),
                        Email = c.String(maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        WordEN = c.String(maxLength: 256, unicode: false),
                        WordCN = c.String(maxLength: 256),
                        WordHK = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.StudentRemarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        IsMissingQualifications = c.Boolean(nullable: false),
                        IsMissingWorkExperience = c.Boolean(nullable: false),
                        IsMissingSupportingDocuments = c.Boolean(nullable: false),
                        IsReject = c.Boolean(nullable: false),
                        Other = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.ApplicationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationStatusStorage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LastModifiedBy = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationStatus", t => t.Status)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.Status);
            
            CreateTable(
                "dbo.EnrollmentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnrollmentStatusStorage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LastModifiedBy = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnrollmentStatus", t => t.Status)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.Status);
            
            CreateTable(
                "dbo.CourseMasterDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.PaymentTransactions", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.LessonAttendances", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Invoices", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.EnrollmentStatusStorage", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.EnrollmentStatusStorage", "Status", "dbo.EnrollmentStatus");
            DropForeignKey("dbo.Applications", "EnrollmentStatus", "dbo.EnrollmentStatus");
            DropForeignKey("dbo.DownloadDocumentTrackings", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationTrans", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationStatusStorage", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationStatusStorage", "Status", "dbo.ApplicationStatus");
            DropForeignKey("dbo.Applications", "Status", "dbo.ApplicationStatus");
            DropForeignKey("dbo.Applications", "StudentPreferredClass", "dbo.Classes");
            DropForeignKey("dbo.Lessons", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Exams", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.SystemPrivileges", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.StudentRemarks", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Lecturers", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Keywords", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.EnquiryEmails", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseTrans", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseDocuments", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.CourseCategorieTrans", "CategoryId", "dbo.CourseCategories");
            DropForeignKey("dbo.WorkExperienceTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Users", "CommunicationLanguage", "dbo.Languages");
            DropForeignKey("dbo.TransactionTypes", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.QualificationsTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.PaymentTransactionTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ParticularTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ModuleTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ModuleTrans", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.LocalizationLabels", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.InvoiceTypes", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.InvoiceItemTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.InvoiceItemTrans", "ItemId", "dbo.InvoiceItems");
            DropForeignKey("dbo.Invoices", "InvoiceType", "dbo.InvoiceTypes");
            DropForeignKey("dbo.InvoiceStatusStorage", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceStatusStorage", "Status", "dbo.InvoiceStatus");
            DropForeignKey("dbo.Invoices", "Status", "dbo.InvoiceStatus");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.ExamTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.EmployerRecommendationTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CourseTypes", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Courses", "CourseTypeId", "dbo.CourseTypes");
            DropForeignKey("dbo.CourseTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CourseLocationTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Lessons", "LocationId", "dbo.CourseLocation");
            DropForeignKey("dbo.LessonAttendances", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.CourseDocuments", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.DownloadDocumentTrackings", "CourseDocumentId", "dbo.CourseDocuments");
            DropForeignKey("dbo.UserDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.WorkExperiences", "UserId", "dbo.Users");
            DropForeignKey("dbo.WorkExperienceTrans", "WorkExperienceId", "dbo.WorkExperiences");
            DropForeignKey("dbo.UserDocuments", "UserId", "dbo.Users");
            DropForeignKey("dbo.SystemPrivileges", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Qualifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.QualificationsTrans", "QualificationId", "dbo.Qualifications");
            DropForeignKey("dbo.PaymentTransactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.PaymentTransactions", "TransactionType", "dbo.TransactionTypes");
            DropForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropForeignKey("dbo.Particular", "Id", "dbo.Users");
            DropForeignKey("dbo.ParticularTrans", "ParticularId", "dbo.Particular");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.LessonAttendances", "UserId", "dbo.Users");
            DropForeignKey("dbo.Lecturers", "UserId", "dbo.Users");
            DropForeignKey("dbo.EmployerRecommendations", "UserId", "dbo.Users");
            DropForeignKey("dbo.EmployerRecommendationTrans", "EmployerRecommendationId", "dbo.EmployerRecommendations");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Applications", "UserId", "dbo.Users");
            DropForeignKey("dbo.CourseDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Exams", "ExamVenue", "dbo.CourseLocation");
            DropForeignKey("dbo.ExamTrans", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.CourseLocationTrans", "CourseLocationId", "dbo.CourseLocation");
            DropForeignKey("dbo.CourseCategorieTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ApplicationTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CourseCategories", "ParentId", "dbo.CourseCategories");
            DropForeignKey("dbo.Classes", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Applications", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Applications", "AdminAssignedClass", "dbo.Classes");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.EnrollmentStatusStorage", new[] { "Status" });
            DropIndex("dbo.EnrollmentStatusStorage", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationStatusStorage", new[] { "Status" });
            DropIndex("dbo.ApplicationStatusStorage", new[] { "ApplicationId" });
            DropIndex("dbo.StudentRemarks", new[] { "CourseId" });
            DropIndex("dbo.Keywords", new[] { "CourseId" });
            DropIndex("dbo.EnquiryEmails", new[] { "CourseId" });
            DropIndex("dbo.Modules", new[] { "CourseId" });
            DropIndex("dbo.ModuleTrans", new[] { "ModuleId" });
            DropIndex("dbo.ModuleTrans", new[] { "LanguageId" });
            DropIndex("dbo.LocalizationLabels", new[] { "LanguageId" });
            DropIndex("dbo.InvoiceTypes", new[] { "LanguageId" });
            DropIndex("dbo.InvoiceStatusStorage", new[] { "Status" });
            DropIndex("dbo.InvoiceStatusStorage", new[] { "InvoiceId" });
            DropIndex("dbo.Invoices", "INDEX_INVOICENUM");
            DropIndex("dbo.Invoices", new[] { "Status" });
            DropIndex("dbo.Invoices", new[] { "InvoiceType" });
            DropIndex("dbo.Invoices", new[] { "ApplicationId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropIndex("dbo.InvoiceItemTrans", new[] { "LanguageId" });
            DropIndex("dbo.InvoiceItemTrans", new[] { "ItemId" });
            DropIndex("dbo.CourseTypes", new[] { "LanguageId" });
            DropIndex("dbo.CourseTrans", new[] { "CourseId" });
            DropIndex("dbo.CourseTrans", new[] { "LanguageId" });
            DropIndex("dbo.DownloadDocumentTrackings", new[] { "ApplicationId" });
            DropIndex("dbo.DownloadDocumentTrackings", new[] { "CourseDocumentId" });
            DropIndex("dbo.WorkExperienceTrans", new[] { "WorkExperienceId" });
            DropIndex("dbo.WorkExperienceTrans", new[] { "LanguageId" });
            DropIndex("dbo.WorkExperiences", new[] { "UserId" });
            DropIndex("dbo.SystemPrivileges", new[] { "CourseId" });
            DropIndex("dbo.SystemPrivileges", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.QualificationsTrans", new[] { "LanguageId" });
            DropIndex("dbo.QualificationsTrans", new[] { "QualificationId" });
            DropIndex("dbo.Qualifications", new[] { "UserId" });
            DropIndex("dbo.TransactionTypes", new[] { "LanguageId" });
            DropIndex("dbo.PaymentTransactionTrans", new[] { "PaymentTransactionId" });
            DropIndex("dbo.PaymentTransactionTrans", new[] { "LanguageId" });
            DropIndex("dbo.PaymentTransactions", new[] { "ApplicationId" });
            DropIndex("dbo.PaymentTransactions", new[] { "UserId" });
            DropIndex("dbo.PaymentTransactions", new[] { "TransactionType" });
            DropIndex("dbo.ParticularTrans", new[] { "LanguageId" });
            DropIndex("dbo.ParticularTrans", new[] { "ParticularId" });
            DropIndex("dbo.Particular", "INDEX_MOBILENUM");
            DropIndex("dbo.Particular", new[] { "Id" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.LessonAttendances", new[] { "UserId" });
            DropIndex("dbo.LessonAttendances", new[] { "ApplicationId" });
            DropIndex("dbo.LessonAttendances", new[] { "LessonId" });
            DropIndex("dbo.Lecturers", new[] { "UserId" });
            DropIndex("dbo.Lecturers", new[] { "CourseId" });
            DropIndex("dbo.EmployerRecommendationTrans", new[] { "EmployerRecommendationId" });
            DropIndex("dbo.EmployerRecommendationTrans", new[] { "LanguageId" });
            DropIndex("dbo.EmployerRecommendations", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "CommunicationLanguage" });
            DropIndex("dbo.Users", "INDEX_CICNUMBER");
            DropIndex("dbo.UserDocuments", new[] { "DocumentId" });
            DropIndex("dbo.UserDocuments", new[] { "UserId" });
            DropIndex("dbo.CourseDocuments", new[] { "LessonId" });
            DropIndex("dbo.CourseDocuments", new[] { "DocumentId" });
            DropIndex("dbo.CourseDocuments", new[] { "CourseId" });
            DropIndex("dbo.Lessons", new[] { "LocationId" });
            DropIndex("dbo.Lessons", new[] { "ClassId" });
            DropIndex("dbo.ExamTrans", new[] { "ExamId" });
            DropIndex("dbo.ExamTrans", new[] { "LanguageId" });
            DropIndex("dbo.Exams", new[] { "ExamVenue" });
            DropIndex("dbo.Exams", new[] { "ClassId" });
            DropIndex("dbo.CourseLocationTrans", new[] { "CourseLocationId" });
            DropIndex("dbo.CourseLocationTrans", new[] { "LanguageId" });
            DropIndex("dbo.ApplicationTrans", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationTrans", new[] { "LanguageId" });
            DropIndex("dbo.CourseCategorieTrans", new[] { "CategoryId" });
            DropIndex("dbo.CourseCategorieTrans", new[] { "LanguageId" });
            DropIndex("dbo.CourseCategories", new[] { "ParentId" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "CourseTypeId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropIndex("dbo.Courses", "INDEX_COURSECODE");
            DropIndex("dbo.Classes", new[] { "CourseId" });
            DropIndex("dbo.Classes", "INDEX_CLASSCODE");
            DropIndex("dbo.Applications", "INDEX_APPNUM");
            DropIndex("dbo.Applications", new[] { "AdminAssignedClass" });
            DropIndex("dbo.Applications", new[] { "StudentPreferredClass" });
            DropIndex("dbo.Applications", new[] { "EnrollmentStatus" });
            DropIndex("dbo.Applications", new[] { "Status" });
            DropIndex("dbo.Applications", new[] { "UserId" });
            DropIndex("dbo.Applications", new[] { "CourseId" });
            DropTable("dbo.Roles");
            DropTable("dbo.CourseMasterDatas");
            DropTable("dbo.EnrollmentStatusStorage");
            DropTable("dbo.EnrollmentStatus");
            DropTable("dbo.ApplicationStatusStorage");
            DropTable("dbo.ApplicationStatus");
            DropTable("dbo.StudentRemarks");
            DropTable("dbo.Keywords");
            DropTable("dbo.EnquiryEmails");
            DropTable("dbo.Modules");
            DropTable("dbo.ModuleTrans");
            DropTable("dbo.LocalizationLabels");
            DropTable("dbo.InvoiceTypes");
            DropTable("dbo.InvoiceStatusStorage");
            DropTable("dbo.InvoiceStatus");
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.InvoiceItemTrans");
            DropTable("dbo.CourseTypes");
            DropTable("dbo.CourseTrans");
            DropTable("dbo.DownloadDocumentTrackings");
            DropTable("dbo.WorkExperienceTrans");
            DropTable("dbo.WorkExperiences");
            DropTable("dbo.SystemPrivileges");
            DropTable("dbo.UserRoles");
            DropTable("dbo.QualificationsTrans");
            DropTable("dbo.Qualifications");
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.PaymentTransactionTrans");
            DropTable("dbo.PaymentTransactions");
            DropTable("dbo.ParticularTrans");
            DropTable("dbo.Particular");
            DropTable("dbo.UserLogins");
            DropTable("dbo.LessonAttendances");
            DropTable("dbo.Lecturers");
            DropTable("dbo.EmployerRecommendationTrans");
            DropTable("dbo.EmployerRecommendations");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.UserDocuments");
            DropTable("dbo.Documents");
            DropTable("dbo.CourseDocuments");
            DropTable("dbo.Lessons");
            DropTable("dbo.ExamTrans");
            DropTable("dbo.Exams");
            DropTable("dbo.CourseLocation");
            DropTable("dbo.CourseLocationTrans");
            DropTable("dbo.ApplicationTrans");
            DropTable("dbo.Languages");
            DropTable("dbo.CourseCategorieTrans");
            DropTable("dbo.CourseCategories");
            DropTable("dbo.Courses");
            DropTable("dbo.Classes");
            DropTable("dbo.Applications");
        }
    }
}
