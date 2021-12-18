namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecmsmatched : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsContents", "MatchedItemId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CmsContents", "MatchedItemId");
        }
    }
}
