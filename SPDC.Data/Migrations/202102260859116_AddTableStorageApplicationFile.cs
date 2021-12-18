namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableStorageApplicationFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationAssessmentDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.ApplicationId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.ApplicationAttendanceDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.ApplicationId)
                .Index(t => t.DocumentId);
            
            AlterColumn("dbo.Applications", "EmailStatus", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationAttendanceDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.ApplicationAssessmentDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.ApplicationAttendanceDocuments", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationAssessmentDocuments", "ApplicationId", "dbo.Applications");
            DropIndex("dbo.ApplicationAttendanceDocuments", new[] { "DocumentId" });
            DropIndex("dbo.ApplicationAttendanceDocuments", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationAssessmentDocuments", new[] { "DocumentId" });
            DropIndex("dbo.ApplicationAssessmentDocuments", new[] { "ApplicationId" });
            AlterColumn("dbo.Applications", "EmailStatus", c => c.Int(nullable: false));
            DropTable("dbo.ApplicationAttendanceDocuments");
            DropTable("dbo.ApplicationAssessmentDocuments");
        }
    }
}
