namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_field_application_table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applications", "ApplicationStatusId", c => c.Int());
            AlterColumn("dbo.Applications", "EnrollmentStatusId", c => c.Int());
            AlterColumn("dbo.Applications", "ApplicationFirstSubmissionDate", c => c.DateTime());
            AlterColumn("dbo.Applications", "ApplicationLastSubmissionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applications", "ApplicationLastSubmissionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Applications", "ApplicationFirstSubmissionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Applications", "EnrollmentStatusId", c => c.Int(nullable: false));
            AlterColumn("dbo.Applications", "ApplicationStatusId", c => c.Int(nullable: false));
        }
    }
}
