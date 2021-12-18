namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtablesrefund : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefundTransactionDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefundTransactionId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RefundTransactions", t => t.RefundTransactionId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.RefundTransactionId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.RefundTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        TransactionTypeId = c.Int(nullable: false),
                        AcceptedBankId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        RefNo = c.String(maxLength: 265),
                        Remarks = c.String(),
                        ReasonForRefund = c.String(),
                        RefundApprovedStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.AcceptedBanks", t => t.AcceptedBankId)
                .Index(t => t.InvoiceId)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.AcceptedBankId);
            
            CreateTable(
                "dbo.RefundTransactionApprovedStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefundTransactionId = c.Int(nullable: false),
                        ApprovalUpdatedBy = c.Int(nullable: false),
                        ApprovalUpdatedTime = c.DateTime(nullable: false),
                        ApprovalStatusFrom = c.Int(nullable: false),
                        ApprovalStatusTo = c.Int(nullable: false),
                        ApprovalRemarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RefundTransactions", t => t.RefundTransactionId, cascadeDelete: true)
                .Index(t => t.RefundTransactionId);
            
            CreateTable(
                "dbo.RefundTransactionHistoryDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefundTransactionApprovedStatusHistoryId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RefundTransactionApprovedStatusHistories", t => t.RefundTransactionApprovedStatusHistoryId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.RefundTransactionApprovedStatusHistoryId)
                .Index(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefundTransactions", "AcceptedBankId", "dbo.AcceptedBanks");
            DropForeignKey("dbo.RefundTransactions", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.RefundTransactionHistoryDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.RefundTransactionDocuments", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.RefundTransactions", "TransactionTypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.RefundTransactionDocuments", "RefundTransactionId", "dbo.RefundTransactions");
            DropForeignKey("dbo.RefundTransactionApprovedStatusHistories", "RefundTransactionId", "dbo.RefundTransactions");
            DropForeignKey("dbo.RefundTransactionHistoryDocuments", "RefundTransactionApprovedStatusHistoryId", "dbo.RefundTransactionApprovedStatusHistories");
            DropIndex("dbo.RefundTransactionHistoryDocuments", new[] { "DocumentId" });
            DropIndex("dbo.RefundTransactionHistoryDocuments", new[] { "RefundTransactionApprovedStatusHistoryId" });
            DropIndex("dbo.RefundTransactionApprovedStatusHistories", new[] { "RefundTransactionId" });
            DropIndex("dbo.RefundTransactions", new[] { "AcceptedBankId" });
            DropIndex("dbo.RefundTransactions", new[] { "TransactionTypeId" });
            DropIndex("dbo.RefundTransactions", new[] { "InvoiceId" });
            DropIndex("dbo.RefundTransactionDocuments", new[] { "DocumentId" });
            DropIndex("dbo.RefundTransactionDocuments", new[] { "RefundTransactionId" });
            DropTable("dbo.RefundTransactionHistoryDocuments");
            DropTable("dbo.RefundTransactionApprovedStatusHistories");
            DropTable("dbo.RefundTransactions");
            DropTable("dbo.RefundTransactionDocuments");
        }
    }
}
