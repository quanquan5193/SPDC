namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCmsimage : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CmsImages", new[] { "CmsId" });
            AlterColumn("dbo.CmsImages", "CmsId", c => c.Int());
            CreateIndex("dbo.CmsImages", "CmsId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CmsImages", new[] { "CmsId" });
            AlterColumn("dbo.CmsImages", "CmsId", c => c.Int(nullable: false));
            CreateIndex("dbo.CmsImages", "CmsId");
        }
    }
}
