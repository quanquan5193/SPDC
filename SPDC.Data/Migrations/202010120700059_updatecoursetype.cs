namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecoursetype : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourseTypes", "LanguageId", "dbo.Languages");
            DropIndex("dbo.CourseTypes", new[] { "LanguageId" });
            AddColumn("dbo.CourseTypes", "NameEN", c => c.String(maxLength: 256));
            AddColumn("dbo.CourseTypes", "NameHK", c => c.String(maxLength: 256));
            AddColumn("dbo.CourseTypes", "NameCN", c => c.String(maxLength: 256));
            DropColumn("dbo.AdminPermissions", "UserId");
            DropColumn("dbo.CourseTypes", "LanguageId");
            DropColumn("dbo.CourseTypes", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourseTypes", "Name", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.CourseTypes", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.AdminPermissions", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.CourseTypes", "NameCN");
            DropColumn("dbo.CourseTypes", "NameHK");
            DropColumn("dbo.CourseTypes", "NameEN");
            CreateIndex("dbo.CourseTypes", "LanguageId");
            AddForeignKey("dbo.CourseTypes", "LanguageId", "dbo.Languages", "Id");
        }
    }
}
