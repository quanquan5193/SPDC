namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterelationshipTargetClassClassCommon : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TargetClasses", "ClassCommonId", "dbo.ClassCommon");
            DropIndex("dbo.TargetClasses", new[] { "ClassCommonId" });
            DropColumn("dbo.TargetClasses", "ClassCommonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TargetClasses", "ClassCommonId", c => c.Int(nullable: false));
            CreateIndex("dbo.TargetClasses", "ClassCommonId");
            AddForeignKey("dbo.TargetClasses", "ClassCommonId", "dbo.ClassCommon", "Id", cascadeDelete: true);
        }
    }
}
