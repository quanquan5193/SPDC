namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTransactionWaivePayment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropForeignKey("dbo.PaymentTransactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.PaymentTransactionTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.TransactionTypes", "LanguageId", "dbo.Languages");
            DropIndex("dbo.PaymentTransactions", new[] { "UserId" });
            DropIndex("dbo.PaymentTransactionTrans", new[] { "LanguageId" });
            DropIndex("dbo.PaymentTransactionTrans", new[] { "PaymentTransactionId" });
            DropIndex("dbo.TransactionTypes", new[] { "LanguageId" });
            CreateTable(
                "dbo.AcceptedBanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEN = c.String(nullable: false, maxLength: 256),
                        NameCN = c.String(nullable: false, maxLength: 256),
                        NameHK = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentTransactionDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentTransactionId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .ForeignKey("dbo.PaymentTransactions", t => t.PaymentTransactionId)
                .Index(t => t.PaymentTransactionId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.WaivedHistoryDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WaiverApprovedStatusHistoryId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WaiverApprovedStatusHistories", t => t.WaiverApprovedStatusHistoryId)
                .ForeignKey("dbo.Documents", t => t.WaiverApprovedStatusHistoryId)
                .Index(t => t.WaiverApprovedStatusHistoryId);
            
            CreateTable(
                "dbo.WaiverApprovedStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        ApprovalUpdatedBy = c.Int(nullable: false),
                        ApprovalUpdatedTime = c.DateTime(nullable: false),
                        ApprovalStatusFrom = c.Int(nullable: false),
                        ApprovalStatusTo = c.Int(nullable: false),
                        ApprovalRemarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId);
            
            AddColumn("dbo.PaymentTransactions", "AcceptedBankId", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentTransactions", "Remarks", c => c.String());
            AddColumn("dbo.PaymentTransactions", "ReasonForRefund", c => c.String());
            AddColumn("dbo.Invoices", "WaiverApprovedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.TransactionTypes", "NameEN", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.TransactionTypes", "NameCN", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.TransactionTypes", "NameHK", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.PaymentTransactions", "AcceptedBankId");
            AddForeignKey("dbo.PaymentTransactions", "AcceptedBankId", "dbo.AcceptedBanks", "Id");
            DropColumn("dbo.PaymentTransactions", "UserId");
            DropColumn("dbo.PaymentTransactions", "ApplicationId");
            DropColumn("dbo.TransactionTypes", "LanguageId");
            DropColumn("dbo.TransactionTypes", "Name");
            DropTable("dbo.PaymentTransactionTrans");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentTransactionTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        PaymentTransactionId = c.Int(nullable: false),
                        BankName = c.String(nullable: false, maxLength: 256),
                        Remarks = c.String(nullable: false, maxLength: 256),
                        ReasonForRefund = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TransactionTypes", "Name", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.TransactionTypes", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentTransactions", "ApplicationId", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentTransactions", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PaymentTransactions", "AcceptedBankId", "dbo.AcceptedBanks");
            DropForeignKey("dbo.PaymentTransactionDocuments", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropForeignKey("dbo.WaiverApprovedStatusHistories", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.WaivedHistoryDocuments", "WaiverApprovedStatusHistoryId", "dbo.Documents");
            DropForeignKey("dbo.WaivedHistoryDocuments", "WaiverApprovedStatusHistoryId", "dbo.WaiverApprovedStatusHistories");
            DropForeignKey("dbo.PaymentTransactionDocuments", "DocumentId", "dbo.Documents");
            DropIndex("dbo.WaiverApprovedStatusHistories", new[] { "InvoiceId" });
            DropIndex("dbo.WaivedHistoryDocuments", new[] { "WaiverApprovedStatusHistoryId" });
            DropIndex("dbo.PaymentTransactionDocuments", new[] { "DocumentId" });
            DropIndex("dbo.PaymentTransactionDocuments", new[] { "PaymentTransactionId" });
            DropIndex("dbo.PaymentTransactions", new[] { "AcceptedBankId" });
            DropColumn("dbo.TransactionTypes", "NameHK");
            DropColumn("dbo.TransactionTypes", "NameCN");
            DropColumn("dbo.TransactionTypes", "NameEN");
            DropColumn("dbo.Invoices", "WaiverApprovedStatus");
            DropColumn("dbo.PaymentTransactions", "ReasonForRefund");
            DropColumn("dbo.PaymentTransactions", "Remarks");
            DropColumn("dbo.PaymentTransactions", "AcceptedBankId");
            DropTable("dbo.WaiverApprovedStatusHistories");
            DropTable("dbo.WaivedHistoryDocuments");
            DropTable("dbo.PaymentTransactionDocuments");
            DropTable("dbo.AcceptedBanks");
            CreateIndex("dbo.TransactionTypes", "LanguageId");
            CreateIndex("dbo.PaymentTransactionTrans", "PaymentTransactionId");
            CreateIndex("dbo.PaymentTransactionTrans", "LanguageId");
            CreateIndex("dbo.PaymentTransactions", "UserId");
            AddForeignKey("dbo.TransactionTypes", "LanguageId", "dbo.Languages", "Id");
            AddForeignKey("dbo.PaymentTransactionTrans", "LanguageId", "dbo.Languages", "Id");
            AddForeignKey("dbo.PaymentTransactions", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions", "Id");
        }
    }
}
