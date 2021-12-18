namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterelationshipcoursetargetclass : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TargetClasses");
            AlterColumn("dbo.TargetClasses", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TargetClasses", "Id");
            CreateIndex("dbo.TargetClasses", "Id");
            AddForeignKey("dbo.TargetClasses", "Id", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TargetClasses", "Id", "dbo.Courses");
            DropIndex("dbo.TargetClasses", new[] { "Id" });
            DropPrimaryKey("dbo.TargetClasses");
            AlterColumn("dbo.TargetClasses", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TargetClasses", "Id");
        }
    }
}
