namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSystemConfig : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemConfig");
        }
    }
}
