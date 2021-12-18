namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_common_data_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommonData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Name = c.String(),
                        ValueString = c.String(),
                        ValueInt = c.Int(nullable: false),
                        ValueDouble = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommonData");
        }
    }
}
