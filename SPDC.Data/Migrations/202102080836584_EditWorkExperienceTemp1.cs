namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditWorkExperienceTemp1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkExperienceTemp", "ApplicationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkExperienceTemp", "ApplicationId");
        }
    }
}
