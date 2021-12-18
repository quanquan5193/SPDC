namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCourseTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EnquiryEmails", "CourseId", "dbo.Courses");
            DropIndex("dbo.EnquiryEmails", new[] { "CourseId" });
            CreateTable(
                "dbo.Enquiries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(),
                        EnquiryNo = c.Int(nullable: false),
                        Email = c.String(maxLength: 256, unicode: false),
                        Phone = c.String(maxLength: 256),
                        Fax = c.String(maxLength: 256),
                        ContactPersonEN = c.String(maxLength: 256),
                        ContactPersonCN = c.String(maxLength: 256),
                        ContactPersonHK = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.ModuleCombinations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        ModuleNo = c.Int(nullable: false),
                        CombinationNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            AddColumn("dbo.CourseTrans", "Recognition", c => c.String(maxLength: 256));
            AddColumn("dbo.CourseTrans", "AdmissionRequirements", c => c.String(maxLength: 256));
            DropColumn("dbo.Courses", "EnquiriesNumber");
            DropTable("dbo.EnquiryEmails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EnquiryEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(),
                        Email = c.String(maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Courses", "EnquiriesNumber", c => c.String(maxLength: 256, unicode: false));
            DropForeignKey("dbo.ModuleCombinations", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Enquiries", "CourseId", "dbo.Courses");
            DropIndex("dbo.ModuleCombinations", new[] { "CourseId" });
            DropIndex("dbo.Enquiries", new[] { "CourseId" });
            DropColumn("dbo.CourseTrans", "AdmissionRequirements");
            DropColumn("dbo.CourseTrans", "Recognition");
            DropTable("dbo.ModuleCombinations");
            DropTable("dbo.Enquiries");
            CreateIndex("dbo.EnquiryEmails", "CourseId");
            AddForeignKey("dbo.EnquiryEmails", "CourseId", "dbo.Courses", "Id");
        }
    }
}
