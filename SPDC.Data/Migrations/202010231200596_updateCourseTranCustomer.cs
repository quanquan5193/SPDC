namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCourseTranCustomer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CourseTrans", "CourseName", c => c.String(nullable: false));
            AlterColumn("dbo.CourseTrans", "CourseTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourseTrans", "CourseTitle", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.CourseTrans", "CourseName", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
