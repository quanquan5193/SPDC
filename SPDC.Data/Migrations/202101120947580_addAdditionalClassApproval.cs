namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdditionalClassApproval : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalClassesApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        OriginalTargetNumber = c.Int(nullable: false),
                        NewTargetNumber = c.Int(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        StatusFrom = c.Int(nullable: false),
                        StatusTo = c.Int(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        ApprovalRemark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.AdditionalClassesApprovalDocument",
                c => new
                    {
                        AdditionalClassesApprovalRefId = c.Int(nullable: false),
                        DocumentRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdditionalClassesApprovalRefId, t.DocumentRefId })
                .ForeignKey("dbo.AdditionalClassesApprovals", t => t.AdditionalClassesApprovalRefId, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.DocumentRefId, cascadeDelete: true)
                .Index(t => t.AdditionalClassesApprovalRefId)
                .Index(t => t.DocumentRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdditionalClassesApprovals", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.AdditionalClassesApprovalDocument", "DocumentRefId", "dbo.Documents");
            DropForeignKey("dbo.AdditionalClassesApprovalDocument", "AdditionalClassesApprovalRefId", "dbo.AdditionalClassesApprovals");
            DropIndex("dbo.AdditionalClassesApprovalDocument", new[] { "DocumentRefId" });
            DropIndex("dbo.AdditionalClassesApprovalDocument", new[] { "AdditionalClassesApprovalRefId" });
            DropIndex("dbo.AdditionalClassesApprovals", new[] { "ClassId" });
            DropTable("dbo.AdditionalClassesApprovalDocument");
            DropTable("dbo.AdditionalClassesApprovals");
        }
    }
}
