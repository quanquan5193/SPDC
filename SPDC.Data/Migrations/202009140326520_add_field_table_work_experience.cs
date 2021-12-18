namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_field_table_work_experience : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkExperiences", "ClassifyWorkingExperience", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkExperiences", "ClassifyWorkingExperience");
        }
    }
}
