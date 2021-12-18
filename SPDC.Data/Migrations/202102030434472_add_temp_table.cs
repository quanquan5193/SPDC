namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_temp_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployerRecommendationTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tel = c.String(nullable: false, maxLength: 256),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmployerRecommendationTempTran",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 256),
                        ContactPerson = c.String(nullable: false, maxLength: 256),
                        LanguageId = c.Int(nullable: false),
                        EmployerRecommendationId = c.Int(nullable: false),
                        Position = c.String(),
                        Email = c.String(),
                        EmployerRecommendationTemp_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployerRecommendationTemp", t => t.EmployerRecommendationTemp_Id)
                .Index(t => t.EmployerRecommendationTemp_Id);
            
            CreateTable(
                "dbo.QualificationTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateObtained = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.QualificationTempTran",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssuingAuthority = c.String(nullable: false, maxLength: 256),
                        LevelAttained = c.String(nullable: false, maxLength: 256),
                        QualificationId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        QualificationTemp_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QualificationTemp", t => t.QualificationTemp_Id)
                .Index(t => t.QualificationTemp_Id);
            
            CreateTable(
                "dbo.UserDocumentTemp",
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
                "dbo.WorkExperienceTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FromYear = c.DateTime(nullable: false),
                        ToYear = c.DateTime(nullable: false),
                        BIMRelated = c.Boolean(nullable: false),
                        ClassifyWorkingExperience = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WorkExperienceTempTran",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        WorkExperienceId = c.Int(nullable: false),
                        Location = c.String(nullable: false, maxLength: 256),
                        JobNature = c.String(maxLength: 256),
                        Position = c.String(nullable: false, maxLength: 256),
                        WorkExperienceTemp_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkExperienceTemp", t => t.WorkExperienceTemp_Id)
                .Index(t => t.WorkExperienceTemp_Id);
            
            CreateTable(
                "dbo.ParticularTemp",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SurnameEN = c.String(nullable: false, maxLength: 256),
                        GivenNameEN = c.String(nullable: false, maxLength: 256),
                        SurnameCN = c.String(maxLength: 256),
                        GivenNameCN = c.String(maxLength: 256),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Boolean(),
                        HKIDNo = c.Binary(maxLength: 66),
                        PassportNo = c.Binary(maxLength: 66),
                        PassportExpiryDate = c.DateTime(storeType: "date"),
                        MobileNumber = c.Binary(maxLength: 66),
                        MobileNumberPrefix = c.String(maxLength: 256),
                        TelNo = c.String(maxLength: 256),
                        FaxNo = c.String(maxLength: 256),
                        RegionEN = c.String(maxLength: 256),
                        RegionCN = c.String(maxLength: 256),
                        DistrictEN = c.String(maxLength: 256),
                        DistrictCN = c.String(maxLength: 256),
                        StreetNumberEN = c.String(maxLength: 256),
                        StreetNumberCN = c.String(maxLength: 256),
                        StreetRoadEN = c.String(maxLength: 256),
                        StreetRoadCN = c.String(maxLength: 256),
                        EstateQuartersVillageEN = c.String(maxLength: 256),
                        EstateQuartersVillageCN = c.String(maxLength: 256),
                        BuildingEN = c.String(maxLength: 256),
                        BuildingCN = c.String(maxLength: 256),
                        FloorEN = c.String(maxLength: 256),
                        FloorCN = c.String(maxLength: 256),
                        RmFtUnitEN = c.String(maxLength: 256),
                        RmFtUnitCN = c.String(maxLength: 256),
                        SameAddress = c.Boolean(nullable: false),
                        ResidentialRegionEN = c.String(maxLength: 256),
                        ResidentialDistrictEN = c.String(maxLength: 256),
                        ResidentialStreetNumberEN = c.String(maxLength: 256),
                        ResidentialStreetRoadEN = c.String(maxLength: 256),
                        ResidentialEstateQuartersVillageEN = c.String(maxLength: 256),
                        ResidentialBuildingEN = c.String(maxLength: 256),
                        ResidentialFloorEN = c.String(maxLength: 256),
                        ResidentialRmFtUnitEN = c.String(maxLength: 256),
                        ResidentialRegionCN = c.String(maxLength: 256),
                        ResidentialDistrictCN = c.String(maxLength: 256),
                        ResidentialStreetNumberCN = c.String(maxLength: 256),
                        ResidentialStreetRoadCN = c.String(maxLength: 256),
                        ResidentialEstateQuartersVillageCN = c.String(maxLength: 256),
                        ResidentialBuildingCN = c.String(maxLength: 256),
                        ResidentialFloorCN = c.String(maxLength: 256),
                        ResidentialRmFtUnitCN = c.String(maxLength: 256),
                        IsPrimamy = c.Boolean(nullable: false),
                        IsSecondary = c.Boolean(nullable: false),
                        IsTechInst = c.Boolean(nullable: false),
                        IsUniversityCollege = c.Boolean(nullable: false),
                        EducationLevelEN = c.String(maxLength: 256),
                        EducationLevelCN = c.String(maxLength: 256),
                        RelatedQualifications1Check = c.Boolean(),
                        RelatedQualifications2Check = c.Boolean(),
                        RelatedQualifications2Year = c.String(maxLength: 256),
                        RelatedQualifications3Check = c.Boolean(),
                        Honorific = c.Int(nullable: false),
                        HKIDNoEncrypted = c.String(),
                        PassportNoEncrypted = c.String(),
                        MobileNumberEncrypted = c.String(),
                        InterestedTypeOfCourse = c.String(maxLength: 256),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.MobileNumber, unique: true, name: "INDEX_MOBILENUM")
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ParticularTempTran",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParticularId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        RelatedQualifications1Text = c.String(maxLength: 256),
                        RelatedQualifications2Text = c.String(maxLength: 256),
                        PresentEmployer = c.String(maxLength: 256),
                        Position = c.String(maxLength: 256),
                        ParticularTemp_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParticularTemp", t => t.ParticularTemp_Id)
                .Index(t => t.ParticularTemp_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParticularTemp", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ParticularTempTran", "ParticularTemp_Id", "dbo.ParticularTemp");
            DropForeignKey("dbo.UserDocumentTemp", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.WorkExperienceTemp", "UserId", "dbo.Users");
            DropForeignKey("dbo.WorkExperienceTempTran", "WorkExperienceTemp_Id", "dbo.WorkExperienceTemp");
            DropForeignKey("dbo.UserDocumentTemp", "UserId", "dbo.Users");
            DropForeignKey("dbo.QualificationTemp", "UserId", "dbo.Users");
            DropForeignKey("dbo.QualificationTempTran", "QualificationTemp_Id", "dbo.QualificationTemp");
            DropForeignKey("dbo.EmployerRecommendationTemp", "UserId", "dbo.Users");
            DropForeignKey("dbo.EmployerRecommendationTempTran", "EmployerRecommendationTemp_Id", "dbo.EmployerRecommendationTemp");
            DropIndex("dbo.ParticularTempTran", new[] { "ParticularTemp_Id" });
            DropIndex("dbo.ParticularTemp", new[] { "User_Id" });
            DropIndex("dbo.ParticularTemp", "INDEX_MOBILENUM");
            DropIndex("dbo.WorkExperienceTempTran", new[] { "WorkExperienceTemp_Id" });
            DropIndex("dbo.WorkExperienceTemp", new[] { "UserId" });
            DropIndex("dbo.UserDocumentTemp", new[] { "DocumentId" });
            DropIndex("dbo.UserDocumentTemp", new[] { "UserId" });
            DropIndex("dbo.QualificationTempTran", new[] { "QualificationTemp_Id" });
            DropIndex("dbo.QualificationTemp", new[] { "UserId" });
            DropIndex("dbo.EmployerRecommendationTempTran", new[] { "EmployerRecommendationTemp_Id" });
            DropIndex("dbo.EmployerRecommendationTemp", new[] { "UserId" });
            DropTable("dbo.ParticularTempTran");
            DropTable("dbo.ParticularTemp");
            DropTable("dbo.WorkExperienceTempTran");
            DropTable("dbo.WorkExperienceTemp");
            DropTable("dbo.UserDocumentTemp");
            DropTable("dbo.QualificationTempTran");
            DropTable("dbo.QualificationTemp");
            DropTable("dbo.EmployerRecommendationTempTran");
            DropTable("dbo.EmployerRecommendationTemp");
        }
    }
}
