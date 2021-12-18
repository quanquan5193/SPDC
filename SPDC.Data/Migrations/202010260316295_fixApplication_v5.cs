namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixApplication_v5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Classes", "INDEX_CLASSCODE");
            AlterColumn("dbo.Classes", "ClassCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
            CreateIndex("dbo.Classes", "ClassCode", unique: true, name: "INDEX_CLASSCODE");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Classes", "INDEX_CLASSCODE");
            AlterColumn("dbo.Classes", "ClassCode", c => c.String(nullable: false, maxLength: 20, unicode: false));
            CreateIndex("dbo.Classes", "ClassCode", unique: true, name: "INDEX_CLASSCODE");
        }
    }
}
