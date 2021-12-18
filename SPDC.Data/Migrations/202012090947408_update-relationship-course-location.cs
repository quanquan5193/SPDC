namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterelationshipcourselocation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CourseVenueId", c => c.Int());
            CreateIndex("dbo.Courses", "CourseVenueId");
            AddForeignKey("dbo.Courses", "CourseVenueId", "dbo.CourseLocation", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "CourseVenueId", "dbo.CourseLocation");
            DropIndex("dbo.Courses", new[] { "CourseVenueId" });
            AlterColumn("dbo.Courses", "CourseVenueId", c => c.Int(nullable: false));
        }
    }
}
