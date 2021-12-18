namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLengthEmailStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applications", "EmailStatus", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applications", "EmailStatus", c => c.String(maxLength: 256));
        }
    }
}
