namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableapplicationapprovehistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvoiceTypes", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ApplicationStatusStorage", "Status", "dbo.ApplicationStatus");
            DropIndex("dbo.InvoiceTypes", new[] { "LanguageId" });
            DropIndex("dbo.ApplicationStatusStorage", new[] { "Status" });
            CreateTable(
                "dbo.ApplicationHistoryDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationHistoryId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationApprovedStatusHistories", t => t.ApplicationHistoryId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.ApplicationHistoryId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.ApplicationApprovedStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ApprovalUpdatedBy = c.Int(nullable: false),
                        AppovalStatusFrom = c.Int(nullable: false),
                        ApprovalStatusTo = c.Int(nullable: false),
                        ApprovalUpdatedTime = c.DateTime(nullable: false),
                        ApprovalRemarks = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            AddColumn("dbo.Applications", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.InvoiceStatus", "LanguageId");
            DropColumn("dbo.InvoiceTypes", "LanguageId");
            DropTable("dbo.ApplicationStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.InvoiceTypes", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.InvoiceStatus", "LanguageId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicationApprovedStatusHistories", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationHistoryDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.ApplicationHistoryDocuments", "ApplicationHistoryId", "dbo.ApplicationApprovedStatusHistories");
            DropIndex("dbo.ApplicationApprovedStatusHistories", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationHistoryDocuments", new[] { "DocumentId" });
            DropIndex("dbo.ApplicationHistoryDocuments", new[] { "ApplicationHistoryId" });
            DropColumn("dbo.Applications", "Status");
            DropTable("dbo.ApplicationApprovedStatusHistories");
            DropTable("dbo.ApplicationHistoryDocuments");
            CreateIndex("dbo.ApplicationStatusStorage", "Status");
            CreateIndex("dbo.InvoiceTypes", "LanguageId");
            AddForeignKey("dbo.ApplicationStatusStorage", "Status", "dbo.ApplicationStatus", "Id");
            AddForeignKey("dbo.InvoiceTypes", "LanguageId", "dbo.Languages", "Id");
        }
    }
}
