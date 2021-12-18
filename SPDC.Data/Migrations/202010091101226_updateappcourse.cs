namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateappcourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassCommon",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TypeName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdminPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                        IsCreateContent = c.Boolean(nullable: false),
                        IsViewContent = c.Boolean(nullable: false),
                        IsEditContent = c.Boolean(nullable: false),
                        IsDeleteContent = c.Boolean(nullable: false),
                        IsApproveContent = c.Boolean(nullable: false),
                        IsUnapproveContent = c.Boolean(nullable: false),
                        IsPublishContent = c.Boolean(nullable: false),
                        IsUnpublishContent = c.Boolean(nullable: false),
                        IsCreateAdmin = c.Boolean(nullable: false),
                        IsSuspendAdmin = c.Boolean(nullable: false),
                        IsActiveAdmin = c.Boolean(nullable: false),
                        IsEditAdmin = c.Boolean(nullable: false),
                        IsAssignAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Classes", "AttendanceRequirement", c => c.Int());
            AddColumn("dbo.Classes", "ClassCommonId", c => c.Int());
            AddColumn("dbo.Classes", "EnrollmentNumber", c => c.Int());
            CreateIndex("dbo.Classes", "ClassCommonId");
            AddForeignKey("dbo.Classes", "ClassCommonId", "dbo.ClassCommon", "Id");
            DropColumn("dbo.SystemPrivileges", "Status");
            DropColumn("dbo.SystemPrivileges", "IsCreateContent");
            DropColumn("dbo.SystemPrivileges", "IsViewContent");
            DropColumn("dbo.SystemPrivileges", "IsEditContent");
            DropColumn("dbo.SystemPrivileges", "IsDeleteContent");
            DropColumn("dbo.SystemPrivileges", "IsApproveContent");
            DropColumn("dbo.SystemPrivileges", "IsUnapproveContent");
            DropColumn("dbo.SystemPrivileges", "IsPublishContent");
            DropColumn("dbo.SystemPrivileges", "IsUnpublishContent");
            DropColumn("dbo.SystemPrivileges", "IsCreateAdmin");
            DropColumn("dbo.SystemPrivileges", "IsSuspendAdmin");
            DropColumn("dbo.SystemPrivileges", "IsActiveAdmin");
            DropColumn("dbo.SystemPrivileges", "IsEditAdmin");
            DropColumn("dbo.SystemPrivileges", "IsAssignAdmin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SystemPrivileges", "IsAssignAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsEditAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsActiveAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsSuspendAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsCreateAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsUnpublishContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsPublishContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsUnapproveContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsApproveContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsDeleteContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsEditContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsViewContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsCreateContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "Status", c => c.Byte(nullable: false));
            DropForeignKey("dbo.AdminPermissions", "Id", "dbo.Users");
            DropForeignKey("dbo.Classes", "ClassCommonId", "dbo.ClassCommon");
            DropIndex("dbo.AdminPermissions", new[] { "Id" });
            DropIndex("dbo.Classes", new[] { "ClassCommonId" });
            DropColumn("dbo.Classes", "EnrollmentNumber");
            DropColumn("dbo.Classes", "ClassCommonId");
            DropColumn("dbo.Classes", "AttendanceRequirement");
            DropTable("dbo.AdminPermissions");
            DropTable("dbo.ClassCommon");
        }
    }
}
