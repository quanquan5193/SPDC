namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdminEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AdminEmail", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AdminEmail");
        }
    }
}
