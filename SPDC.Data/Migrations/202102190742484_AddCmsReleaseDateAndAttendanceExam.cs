namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCmsReleaseDateAndAttendanceExam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MakeUpAttendences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        MakeUpClassId = c.Int(nullable: false),
                        IsDisplayToStudentPortal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MakeUpClasses", t => t.MakeUpClassId)
                .ForeignKey("dbo.Lessons", t => t.LessonId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.LessonId)
                .Index(t => t.ApplicationId)
                .Index(t => t.MakeUpClassId);
            
            CreateTable(
                "dbo.MakeUpClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TimeFromMin = c.Int(nullable: false),
                        TimeFromSec = c.Int(nullable: false),
                        TimeToMin = c.Int(nullable: false),
                        TimeToSec = c.Int(nullable: false),
                        Venue = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResitExamApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ResitExamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResitExames", t => t.ResitExamId, cascadeDelete: true)
                .ForeignKey("dbo.Lessons", t => t.LessonId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.LessonId)
                .Index(t => t.ApplicationId)
                .Index(t => t.ResitExamId);
            
            CreateTable(
                "dbo.ResitExames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TimeFromMin = c.Int(nullable: false),
                        TimeFromSec = c.Int(nullable: false),
                        TimeToMin = c.Int(nullable: false),
                        TimeToSec = c.Int(nullable: false),
                        Venue = c.String(),
                        Name = c.String(),
                        TypeOfReExam = c.Int(nullable: false),
                        ResitExamApplicationDeadline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExamApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ExamId = c.Int(nullable: false),
                        AssessmentMark = c.Int(nullable: false),
                        AssessmentResult = c.Int(nullable: false),
                        IsMakeUp = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.ExamId);
            
            CreateTable(
                "dbo.MakeUpClassDocument",
                c => new
                    {
                        MakeUpClassRefId = c.Int(nullable: false),
                        DocumentRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MakeUpClassRefId, t.DocumentRefId })
                .ForeignKey("dbo.Documents", t => t.MakeUpClassRefId, cascadeDelete: true)
                .ForeignKey("dbo.MakeUpClasses", t => t.DocumentRefId, cascadeDelete: true)
                .Index(t => t.MakeUpClassRefId)
                .Index(t => t.DocumentRefId);
            
            AddColumn("dbo.Applications", "RemarksAttendance", c => c.String());
            AddColumn("dbo.Applications", "EligibleForExam", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "EligibleForMakeUpClass", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "EligibleForAttendanceCertification", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "AttendanceCertificateIssueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Applications", "EligibleForCourseCompletionCertification", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "CourseCompletionCertificateIssueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Applications", "CreditAcquired", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "RemarksExam", c => c.String());
            AddColumn("dbo.Applications", "EligibleForResitExam", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "FirstExamStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "SecondExamStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "EmailStatus", c => c.Int(nullable: false));
            AddColumn("dbo.LessonAttendances", "IsMakeUp", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmsContents", "ReleaseDate", c => c.DateTime());
            AddColumn("dbo.CmsContents", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MakeUpClassDocument", "DocumentRefId", "dbo.MakeUpClasses");
            DropForeignKey("dbo.MakeUpClassDocument", "MakeUpClassRefId", "dbo.Documents");
            DropForeignKey("dbo.ResitExamApplications", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.MakeUpAttendences", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ExamApplications", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ExamApplications", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.ResitExamApplications", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.ResitExamApplications", "ResitExamId", "dbo.ResitExames");
            DropForeignKey("dbo.MakeUpAttendences", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.MakeUpAttendences", "MakeUpClassId", "dbo.MakeUpClasses");
            DropIndex("dbo.MakeUpClassDocument", new[] { "DocumentRefId" });
            DropIndex("dbo.MakeUpClassDocument", new[] { "MakeUpClassRefId" });
            DropIndex("dbo.ExamApplications", new[] { "ExamId" });
            DropIndex("dbo.ExamApplications", new[] { "ApplicationId" });
            DropIndex("dbo.ResitExamApplications", new[] { "ResitExamId" });
            DropIndex("dbo.ResitExamApplications", new[] { "ApplicationId" });
            DropIndex("dbo.ResitExamApplications", new[] { "LessonId" });
            DropIndex("dbo.MakeUpAttendences", new[] { "MakeUpClassId" });
            DropIndex("dbo.MakeUpAttendences", new[] { "ApplicationId" });
            DropIndex("dbo.MakeUpAttendences", new[] { "LessonId" });
            DropColumn("dbo.CmsContents", "EndDate");
            DropColumn("dbo.CmsContents", "ReleaseDate");
            DropColumn("dbo.LessonAttendances", "IsMakeUp");
            DropColumn("dbo.Applications", "EmailStatus");
            DropColumn("dbo.Applications", "SecondExamStatus");
            DropColumn("dbo.Applications", "FirstExamStatus");
            DropColumn("dbo.Applications", "EligibleForResitExam");
            DropColumn("dbo.Applications", "RemarksExam");
            DropColumn("dbo.Applications", "CreditAcquired");
            DropColumn("dbo.Applications", "CourseCompletionCertificateIssueDate");
            DropColumn("dbo.Applications", "EligibleForCourseCompletionCertification");
            DropColumn("dbo.Applications", "AttendanceCertificateIssueDate");
            DropColumn("dbo.Applications", "EligibleForAttendanceCertification");
            DropColumn("dbo.Applications", "EligibleForMakeUpClass");
            DropColumn("dbo.Applications", "EligibleForExam");
            DropColumn("dbo.Applications", "RemarksAttendance");
            DropTable("dbo.MakeUpClassDocument");
            DropTable("dbo.ExamApplications");
            DropTable("dbo.ResitExames");
            DropTable("dbo.ResitExamApplications");
            DropTable("dbo.MakeUpClasses");
            DropTable("dbo.MakeUpAttendences");
        }
    }
}
