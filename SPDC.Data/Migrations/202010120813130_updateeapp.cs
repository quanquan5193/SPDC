namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateeapp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "ModifiedDate");
        }
    }
}
