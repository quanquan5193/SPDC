namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcourseclasssetupapproval : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubClassDrafts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        NewClassCode = c.String(maxLength: 50),
                        NewAttendanceRequirement = c.Int(),
                        NewAttendanceRequirementTypeId = c.Int(),
                        NewClassCommencementDate = c.DateTime(),
                        NewClassCompletionDate = c.DateTime(),
                        NewClassCapacity = c.Int(),
                        NewClassStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassCommon", t => t.NewAttendanceRequirementTypeId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.ClassId)
                .Index(t => t.NewAttendanceRequirementTypeId);
            
            CreateTable(
                "dbo.SubClassApprovedStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        ApprovalUpdatedBy = c.Int(nullable: false),
                        ApprovalStatusFrom = c.Int(nullable: false),
                        ApprovalStatusTo = c.Int(nullable: false),
                        ApprovalUpdatedTime = c.DateTime(nullable: false),
                        Remarks = c.String(maxLength: 300),
                        SubClassDraftId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubClassDrafts", t => t.SubClassDraftId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.ClassId)
                .Index(t => t.SubClassDraftId);
            
            CreateTable(
                "dbo.SubClassHistoryDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubClassHistoryId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .ForeignKey("dbo.SubClassApprovedStatusHistories", t => t.SubClassHistoryId)
                .Index(t => t.SubClassHistoryId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.ClassHistoryDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassHistoryId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassAppovedStatusHistories", t => t.ClassHistoryId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.ClassHistoryId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.ClassAppovedStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        ApprovalUpdatedBy = c.Int(nullable: false),
                        AppovalStatusFrom = c.Int(nullable: false),
                        ApprovalStatusTo = c.Int(nullable: false),
                        ApprovalUpdatedTime = c.DateTime(nullable: false),
                        ApprovalRemarks = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseAppovedStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        ApprovalUpdatedBy = c.Int(nullable: false),
                        AppovalStatusFrom = c.Int(nullable: false),
                        ApprovalStatusTo = c.Int(nullable: false),
                        ApprovalUpdatedTime = c.DateTime(nullable: false),
                        ApprovalRemarks = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseHistoryDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseHistoryId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseAppovedStatusHistories", t => t.CourseHistoryId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.CourseHistoryId)
                .Index(t => t.DocumentId);
            
            AddColumn("dbo.Classes", "SubClassStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "SubClassApprovedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "CourseApprovedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "ClassApprovedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "IsSetApplicationSetup", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsUserCalendar", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsSubmitAndCancelCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsFirstApproveAndRejectCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsSecondpproveAndRejectCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsThirdApproveAndRejectCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsSubmitAndCancelClass", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsFirstApproveAndRejectClass", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsSecondpproveAndRejectClass", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsThirdApproveAndRejectClass", c => c.Boolean(nullable: false));
            AlterColumn("dbo.CourseDocuments", "DistinguishDocType", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubClassDrafts", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.SubClassApprovedStatusHistories", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.SubClassDrafts", "NewAttendanceRequirementTypeId", "dbo.ClassCommon");
            DropForeignKey("dbo.SubClassApprovedStatusHistories", "SubClassDraftId", "dbo.SubClassDrafts");
            DropForeignKey("dbo.SubClassHistoryDocuments", "SubClassHistoryId", "dbo.SubClassApprovedStatusHistories");
            DropForeignKey("dbo.SubClassHistoryDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.CourseHistoryDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.ClassHistoryDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.CourseAppovedStatusHistories", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseHistoryDocuments", "CourseHistoryId", "dbo.CourseAppovedStatusHistories");
            DropForeignKey("dbo.ClassAppovedStatusHistories", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.ClassHistoryDocuments", "ClassHistoryId", "dbo.ClassAppovedStatusHistories");
            DropIndex("dbo.CourseHistoryDocuments", new[] { "DocumentId" });
            DropIndex("dbo.CourseHistoryDocuments", new[] { "CourseHistoryId" });
            DropIndex("dbo.CourseAppovedStatusHistories", new[] { "CourseId" });
            DropIndex("dbo.ClassAppovedStatusHistories", new[] { "CourseId" });
            DropIndex("dbo.ClassHistoryDocuments", new[] { "DocumentId" });
            DropIndex("dbo.ClassHistoryDocuments", new[] { "ClassHistoryId" });
            DropIndex("dbo.SubClassHistoryDocuments", new[] { "DocumentId" });
            DropIndex("dbo.SubClassHistoryDocuments", new[] { "SubClassHistoryId" });
            DropIndex("dbo.SubClassApprovedStatusHistories", new[] { "SubClassDraftId" });
            DropIndex("dbo.SubClassApprovedStatusHistories", new[] { "ClassId" });
            DropIndex("dbo.SubClassDrafts", new[] { "NewAttendanceRequirementTypeId" });
            DropIndex("dbo.SubClassDrafts", new[] { "ClassId" });
            AlterColumn("dbo.CourseDocuments", "DistinguishDocType", c => c.Int(nullable: false));
            DropColumn("dbo.SystemPrivileges", "IsThirdApproveAndRejectClass");
            DropColumn("dbo.SystemPrivileges", "IsSecondpproveAndRejectClass");
            DropColumn("dbo.SystemPrivileges", "IsFirstApproveAndRejectClass");
            DropColumn("dbo.SystemPrivileges", "IsSubmitAndCancelClass");
            DropColumn("dbo.SystemPrivileges", "IsThirdApproveAndRejectCourse");
            DropColumn("dbo.SystemPrivileges", "IsSecondpproveAndRejectCourse");
            DropColumn("dbo.SystemPrivileges", "IsFirstApproveAndRejectCourse");
            DropColumn("dbo.SystemPrivileges", "IsSubmitAndCancelCourse");
            DropColumn("dbo.SystemPrivileges", "IsUserCalendar");
            DropColumn("dbo.Courses", "IsSetApplicationSetup");
            DropColumn("dbo.Courses", "ClassApprovedStatus");
            DropColumn("dbo.Courses", "CourseApprovedStatus");
            DropColumn("dbo.Classes", "SubClassApprovedStatus");
            DropColumn("dbo.Classes", "SubClassStatus");
            DropTable("dbo.CourseHistoryDocuments");
            DropTable("dbo.CourseAppovedStatusHistories");
            DropTable("dbo.ClassAppovedStatusHistories");
            DropTable("dbo.ClassHistoryDocuments");
            DropTable("dbo.SubClassHistoryDocuments");
            DropTable("dbo.SubClassApprovedStatusHistories");
            DropTable("dbo.SubClassDrafts");
        }
    }
}
