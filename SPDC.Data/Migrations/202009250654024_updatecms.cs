namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsContents", "ApproveStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmsContents", "PublishStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.CmsContents", "LastPublishDate", c => c.DateTime());
            AlterColumn("dbo.CmsContents", "Title", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.CmsContents", "ShowOnLandingPage", c => c.Boolean(nullable: false));
            AlterColumn("dbo.CmsContents", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CmsContents", "CreateBy", c => c.Int(nullable: false));
            AlterColumn("dbo.CmsContents", "LastModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CmsContents", "LastModifiedBy", c => c.Int(nullable: false));
            DropColumn("dbo.CmsContents", "Status");
            DropColumn("dbo.CmsContents", "DeleteDate");
            DropColumn("dbo.CmsContents", "DeleteBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CmsContents", "DeleteBy", c => c.Int());
            AddColumn("dbo.CmsContents", "DeleteDate", c => c.DateTime());
            AddColumn("dbo.CmsContents", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.CmsContents", "LastModifiedBy", c => c.Int());
            AlterColumn("dbo.CmsContents", "LastModifiedDate", c => c.DateTime());
            AlterColumn("dbo.CmsContents", "CreateBy", c => c.Int());
            AlterColumn("dbo.CmsContents", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.CmsContents", "ShowOnLandingPage", c => c.Boolean());
            AlterColumn("dbo.CmsContents", "Title", c => c.String(maxLength: 256));
            DropColumn("dbo.CmsContents", "LastPublishDate");
            DropColumn("dbo.CmsContents", "PublishStatus");
            DropColumn("dbo.CmsContents", "ApproveStatus");
        }
    }
}
