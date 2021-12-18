namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTotalHrs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "DurationTotal", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "DurationTotal");
        }
    }
}
