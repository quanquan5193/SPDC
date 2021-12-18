namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixApplication_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationSetups", "ShowReceipt", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationSetups", "FundingSchema", c => c.String());
            AddColumn("dbo.RelevantWorks", "ShowLetterTemplate", c => c.Boolean(nullable: false));
            AddColumn("dbo.RelevantWorks", "Specify", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RelevantWorks", "Specify");
            DropColumn("dbo.RelevantWorks", "ShowLetterTemplate");
            DropColumn("dbo.ApplicationSetups", "FundingSchema");
            DropColumn("dbo.ApplicationSetups", "ShowReceipt");
        }
    }
}
