namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_field_re_exam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CanApplyForReExam", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "ReExamFee", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Courses", "ReExamRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "ReExamRemarks");
            DropColumn("dbo.Courses", "ReExamFee");
            DropColumn("dbo.Courses", "CanApplyForReExam");
        }
    }
}
