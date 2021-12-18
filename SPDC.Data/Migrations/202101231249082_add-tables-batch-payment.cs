namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtablesbatchpayment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionTypeId = c.Int(nullable: false),
                        AcceptedBankId = c.Int(nullable: false),
                        RefNo = c.String(nullable: false, maxLength: 265),
                        PaymentDate = c.DateTime(nullable: false),
                        Payee = c.String(nullable: false, maxLength: 265),
                        Remarks = c.String(),
                        ListApplication = c.String(),
                        TotalCount = c.Int(nullable: false),
                        IsSettled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId)
                .ForeignKey("dbo.AcceptedBanks", t => t.AcceptedBankId)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.AcceptedBankId);
            
            CreateTable(
                "dbo.BatchPaymentDocument",
                c => new
                    {
                        BatchPaymentId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BatchPaymentId, t.DocumentId })
                .ForeignKey("dbo.BatchPayments", t => t.BatchPaymentId, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.BatchPaymentId)
                .Index(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BatchPayments", "AcceptedBankId", "dbo.AcceptedBanks");
            DropForeignKey("dbo.BatchPaymentDocument", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.BatchPaymentDocument", "BatchPaymentId", "dbo.BatchPayments");
            DropForeignKey("dbo.BatchPayments", "TransactionTypeId", "dbo.TransactionTypes");
            DropIndex("dbo.BatchPaymentDocument", new[] { "DocumentId" });
            DropIndex("dbo.BatchPaymentDocument", new[] { "BatchPaymentId" });
            DropIndex("dbo.BatchPayments", new[] { "AcceptedBankId" });
            DropIndex("dbo.BatchPayments", new[] { "TransactionTypeId" });
            DropTable("dbo.BatchPaymentDocument");
            DropTable("dbo.BatchPayments");
        }
    }
}
