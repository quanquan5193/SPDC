namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_attendance_relationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationAttendanceDocuments", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationAttendanceDocuments", "DocumentId", "dbo.Documents");
            DropIndex("dbo.ApplicationAttendanceDocuments", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationAttendanceDocuments", new[] { "DocumentId" });
            CreateTable(
                "dbo.ApplicationAttendanceDocument",
                c => new
                    {
                        DocumentRefId = c.Int(nullable: false),
                        ApplicationRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentRefId, t.ApplicationRefId })
                .ForeignKey("dbo.Documents", t => t.DocumentRefId, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.ApplicationRefId, cascadeDelete: true)
                .Index(t => t.DocumentRefId)
                .Index(t => t.ApplicationRefId);
            
            AddColumn("dbo.Applications", "AttendanceMarks", c => c.Int());
            DropTable("dbo.ApplicationAttendanceDocuments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationAttendanceDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ApplicationAttendanceDocument", "ApplicationRefId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationAttendanceDocument", "DocumentRefId", "dbo.Documents");
            DropIndex("dbo.ApplicationAttendanceDocument", new[] { "ApplicationRefId" });
            DropIndex("dbo.ApplicationAttendanceDocument", new[] { "DocumentRefId" });
            DropColumn("dbo.Applications", "AttendanceMarks");
            DropTable("dbo.ApplicationAttendanceDocument");
            CreateIndex("dbo.ApplicationAttendanceDocuments", "DocumentId");
            CreateIndex("dbo.ApplicationAttendanceDocuments", "ApplicationId");
            AddForeignKey("dbo.ApplicationAttendanceDocuments", "DocumentId", "dbo.Documents", "Id");
            AddForeignKey("dbo.ApplicationAttendanceDocuments", "ApplicationId", "dbo.Applications", "Id");
        }
    }
}
