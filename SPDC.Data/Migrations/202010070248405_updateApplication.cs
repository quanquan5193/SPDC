namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateApplication : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Applications", "Status", "dbo.ApplicationStatus");
            DropForeignKey("dbo.Applications", "EnrollmentStatus", "dbo.EnrollmentStatus");
            DropIndex("dbo.Applications", new[] { "Status" });
            DropIndex("dbo.Applications", new[] { "EnrollmentStatus" });
            AddColumn("dbo.Applications", "ApplicationStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "EnrollmentStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.SystemPrivileges", "Status", c => c.Byte(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsCreateContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsViewContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsEditContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsDeleteContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsApproveContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsUnapproveContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsPublishContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsUnpublishContent", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsCreateCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsViewCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsEditCourse", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsCreateAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsSuspendAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsActiveAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsEditAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "IsAssignAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.CourseDocuments", "LessonId", c => c.Int());
            DropColumn("dbo.Applications", "Status");
            DropColumn("dbo.Applications", "EnrollmentStatus");
            DropColumn("dbo.SystemPrivileges", "FirstApproved");
            DropColumn("dbo.SystemPrivileges", "SecondApproved");
            DropColumn("dbo.SystemPrivileges", "Other");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SystemPrivileges", "Other", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "SecondApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemPrivileges", "FirstApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Applications", "EnrollmentStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.CourseDocuments", "LessonId");
            DropColumn("dbo.SystemPrivileges", "IsAssignAdmin");
            DropColumn("dbo.SystemPrivileges", "IsEditAdmin");
            DropColumn("dbo.SystemPrivileges", "IsActiveAdmin");
            DropColumn("dbo.SystemPrivileges", "IsSuspendAdmin");
            DropColumn("dbo.SystemPrivileges", "IsCreateAdmin");
            DropColumn("dbo.SystemPrivileges", "IsEditCourse");
            DropColumn("dbo.SystemPrivileges", "IsViewCourse");
            DropColumn("dbo.SystemPrivileges", "IsCreateCourse");
            DropColumn("dbo.SystemPrivileges", "IsUnpublishContent");
            DropColumn("dbo.SystemPrivileges", "IsPublishContent");
            DropColumn("dbo.SystemPrivileges", "IsUnapproveContent");
            DropColumn("dbo.SystemPrivileges", "IsApproveContent");
            DropColumn("dbo.SystemPrivileges", "IsDeleteContent");
            DropColumn("dbo.SystemPrivileges", "IsEditContent");
            DropColumn("dbo.SystemPrivileges", "IsViewContent");
            DropColumn("dbo.SystemPrivileges", "IsCreateContent");
            DropColumn("dbo.SystemPrivileges", "Status");
            DropColumn("dbo.Applications", "EnrollmentStatusId");
            DropColumn("dbo.Applications", "ApplicationStatusId");
            CreateIndex("dbo.Applications", "EnrollmentStatus");
            CreateIndex("dbo.Applications", "Status");
            AddForeignKey("dbo.Applications", "EnrollmentStatus", "dbo.EnrollmentStatus", "Id");
            AddForeignKey("dbo.Applications", "Status", "dbo.ApplicationStatus", "Id");
        }
    }
}
