namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixrelationshiptargetclass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classes", "TargetClassId", "dbo.TargetClasses");
            DropIndex("dbo.Classes", new[] { "TargetClassId" });
            AddColumn("dbo.TargetClasses", "AcademicYear", c => c.String(maxLength: 256, unicode: false));
            DropColumn("dbo.Classes", "AcademicYear");
            DropColumn("dbo.Classes", "TargetClassId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "TargetClassId", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "AcademicYear", c => c.String(maxLength: 256, unicode: false));
            DropColumn("dbo.TargetClasses", "AcademicYear");
            CreateIndex("dbo.Classes", "TargetClassId");
            AddForeignKey("dbo.Classes", "TargetClassId", "dbo.TargetClasses", "Id");
        }
    }
}
