namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appsetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassCommonId = c.Int(nullable: false),
                        TargetNumberClass = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassCommon", t => t.ClassCommonId, cascadeDelete: true)
                .Index(t => t.ClassCommonId);
            
            AddColumn("dbo.Classes", "TargetClassId", c => c.Int(nullable: false));
            AddColumn("dbo.ClassCommon", "TypeCommon", c => c.Int());
            CreateIndex("dbo.Classes", "TargetClassId");
            AddForeignKey("dbo.Classes", "TargetClassId", "dbo.TargetClasses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Classes", "TargetClassId", "dbo.TargetClasses");
            DropForeignKey("dbo.TargetClasses", "ClassCommonId", "dbo.ClassCommon");
            DropIndex("dbo.TargetClasses", new[] { "ClassCommonId" });
            DropIndex("dbo.Classes", new[] { "TargetClassId" });
            DropColumn("dbo.ClassCommon", "TypeCommon");
            DropColumn("dbo.Classes", "TargetClassId");
            DropTable("dbo.TargetClasses");
        }
    }
}
