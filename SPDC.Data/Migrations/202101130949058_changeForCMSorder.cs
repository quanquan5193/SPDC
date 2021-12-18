namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeForCMSorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsContents", "OrderNumber", c => c.Int());
            DropTable("dbo.SystemConfig");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SystemConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEN = c.String(),
                        NameCN = c.String(),
                        NameTC = c.String(),
                        Key = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.CmsContents", "OrderNumber");
        }
    }
}
