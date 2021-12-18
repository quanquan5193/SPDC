namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_from_year_experience : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkExperiences", "FromYear", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkExperiences", "ToYear", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkExperiences", "ToYear", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AlterColumn("dbo.WorkExperiences", "FromYear", c => c.String(nullable: false, maxLength: 256, unicode: false));
        }
    }
}
