namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmsContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContentTypeId = c.Int(nullable: false),
                        Title = c.String(maxLength: 256),
                        SEOUrlLink = c.String(maxLength: 256),
                        Status = c.Int(nullable: false),
                        ImageSEO = c.String(maxLength: 256),
                        AnnoucementDate = c.DateTime(),
                        Description = c.String(),
                        FullDescription = c.String(),
                        ShortDescription = c.String(),
                        ShowOnLandingPage = c.Boolean(),
                        CreateDate = c.DateTime(),
                        CreateBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        DeleteBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CmsContentTypes", t => t.ContentTypeId)
                .Index(t => t.ContentTypeId);
            
            CreateTable(
                "dbo.CmsContentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        CmsType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CmsImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CmsId = c.Int(nullable: false),
                        Url = c.String(nullable: false, maxLength: 256),
                        ContentType = c.String(nullable: false, maxLength: 256),
                        FileName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CmsContents", t => t.CmsId)
                .Index(t => t.CmsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CmsImages", "CmsId", "dbo.CmsContents");
            DropForeignKey("dbo.CmsContents", "ContentTypeId", "dbo.CmsContentTypes");
            DropIndex("dbo.CmsImages", new[] { "CmsId" });
            DropIndex("dbo.CmsContents", new[] { "ContentTypeId" });
            DropTable("dbo.CmsImages");
            DropTable("dbo.CmsContentTypes");
            DropTable("dbo.CmsContents");
        }
    }
}
