namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCMCContentStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsContents", "CmsStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CmsContents", "CmsStatus");
        }
    }
}
