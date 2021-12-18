namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_axem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ResitExamApplications", "LessonId", "dbo.Lessons");
            DropIndex("dbo.ResitExamApplications", new[] { "LessonId" });
            AddColumn("dbo.ResitExamApplications", "Exam_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Applications", "AttendanceCertificateIssueDate", c => c.DateTime());
            AlterColumn("dbo.Applications", "CourseCompletionCertificateIssueDate", c => c.DateTime());
            CreateIndex("dbo.ResitExamApplications", "Exam_Id");
            AddForeignKey("dbo.ResitExamApplications", "Exam_Id", "dbo.Exams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResitExamApplications", "Exam_Id", "dbo.Exams");
            DropIndex("dbo.ResitExamApplications", new[] { "Exam_Id" });
            AlterColumn("dbo.Applications", "CourseCompletionCertificateIssueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Applications", "AttendanceCertificateIssueDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.ResitExamApplications", "Exam_Id");
            CreateIndex("dbo.ResitExamApplications", "LessonId");
            AddForeignKey("dbo.ResitExamApplications", "LessonId", "dbo.Lessons", "Id");
        }
    }
}
