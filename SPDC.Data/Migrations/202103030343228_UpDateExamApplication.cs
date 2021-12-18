namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpDateExamApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamApplications", "FromExamId", c => c.Int());
            CreateIndex("dbo.ExamApplications", "FromExamId");
            AddForeignKey("dbo.ExamApplications", "FromExamId", "dbo.Exams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamApplications", "FromExamId", "dbo.Exams");
            DropIndex("dbo.ExamApplications", new[] { "FromExamId" });
            DropColumn("dbo.ExamApplications", "FromExamId");
        }
    }
}
