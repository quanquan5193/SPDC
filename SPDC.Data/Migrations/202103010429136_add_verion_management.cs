namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_verion_management : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VersionManagement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        Remark = c.String(),
                        PublishDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VersionManagement");
        }
    }
}
