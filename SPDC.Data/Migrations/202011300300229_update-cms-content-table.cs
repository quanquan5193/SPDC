namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecmscontenttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsContents", "ApplyingFor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CmsContents", "ApplyingFor");
        }
    }
}
