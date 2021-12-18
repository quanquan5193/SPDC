namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateadminpermissionmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminPermissions", "IsBatchApplication", c => c.Boolean(nullable: false));
            AddColumn("dbo.AdminPermissions", "IsBatchPayment", c => c.Boolean(nullable: false));
            AddColumn("dbo.AdminPermissions", "IsAttendance", c => c.Boolean(nullable: false));
            AddColumn("dbo.AdminPermissions", "IsAssessment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdminPermissions", "IsAssessment");
            DropColumn("dbo.AdminPermissions", "IsAttendance");
            DropColumn("dbo.AdminPermissions", "IsBatchPayment");
            DropColumn("dbo.AdminPermissions", "IsBatchApplication");
        }
    }
}
