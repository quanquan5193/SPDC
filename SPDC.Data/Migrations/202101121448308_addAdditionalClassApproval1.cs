namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdditionalClassApproval1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdditionalClassesApprovals", "ClassId", "dbo.Classes");
            DropIndex("dbo.AdditionalClassesApprovals", new[] { "ClassId" });
            AddColumn("dbo.AdditionalClassesApprovals", "CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.AdditionalClassesApprovals", "CourseId");
            AddForeignKey("dbo.AdditionalClassesApprovals", "CourseId", "dbo.Courses", "Id");
            DropColumn("dbo.AdditionalClassesApprovals", "ClassId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdditionalClassesApprovals", "ClassId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AdditionalClassesApprovals", "CourseId", "dbo.Courses");
            DropIndex("dbo.AdditionalClassesApprovals", new[] { "CourseId" });
            DropColumn("dbo.AdditionalClassesApprovals", "CourseId");
            CreateIndex("dbo.AdditionalClassesApprovals", "ClassId");
            AddForeignKey("dbo.AdditionalClassesApprovals", "ClassId", "dbo.Classes", "Id");
        }
    }
}
