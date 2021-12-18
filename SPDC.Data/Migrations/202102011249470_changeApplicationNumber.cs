namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeApplicationNumber : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Applications", "INDEX_APPNUM");
            AlterColumn("dbo.Applications", "ApplicationNumber", c => c.String(maxLength: 256, unicode: false));
            CreateIndex("dbo.Applications", "ApplicationNumber", unique: true, name: "INDEX_APPNUM");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Applications", "INDEX_APPNUM");
            AlterColumn("dbo.Applications", "ApplicationNumber", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("dbo.Applications", "ApplicationNumber", unique: true, name: "INDEX_APPNUM");
        }
    }
}
