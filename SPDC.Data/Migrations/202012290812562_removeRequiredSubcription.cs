namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequiredSubcription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserSubscriptions", "FirstNameCN", c => c.String(maxLength: 256));
            AlterColumn("dbo.UserSubscriptions", "LastNameCN", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserSubscriptions", "LastNameCN", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserSubscriptions", "FirstNameCN", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
