namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addthreefieldstoapplicationtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "IHaveApplyFor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "IHaveApplyForText", c => c.String());
            AddColumn("dbo.Applications", "IsRequiredRecipt", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "IsRequiredRecipt");
            DropColumn("dbo.Applications", "IHaveApplyForText");
            DropColumn("dbo.Applications", "IHaveApplyFor");
        }
    }
}
