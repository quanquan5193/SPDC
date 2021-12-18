namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_submission_date_application_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "ApplicationFirstSubmissionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Applications", "ApplicationLastSubmissionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "ApplicationLastSubmissionDate");
            DropColumn("dbo.Applications", "ApplicationFirstSubmissionDate");
        }
    }
}
