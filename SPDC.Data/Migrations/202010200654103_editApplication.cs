namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseLocation", "VenueCode", c => c.String());
            AddColumn("dbo.TargetClasses", "CourseId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TargetClasses", "CourseId");
            DropColumn("dbo.CourseLocation", "VenueCode");
        }
    }
}
