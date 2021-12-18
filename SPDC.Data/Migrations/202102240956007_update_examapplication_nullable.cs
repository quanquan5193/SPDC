namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_examapplication_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExamApplications", "AssessmentMark", c => c.Int());
            AlterColumn("dbo.ExamApplications", "AssessmentResult", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExamApplications", "AssessmentResult", c => c.Int(nullable: false));
            AlterColumn("dbo.ExamApplications", "AssessmentMark", c => c.Int(nullable: false));
        }
    }
}
