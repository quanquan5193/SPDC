namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelationshippaymentinvoice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentTransactions", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropIndex("dbo.PaymentTransactions", new[] { "ApplicationId" });
            DropPrimaryKey("dbo.PaymentTransactions");
            AddColumn("dbo.PaymentTransactions", "InvoiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.PaymentTransactions", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PaymentTransactions", "Id");
            CreateIndex("dbo.PaymentTransactions", "InvoiceId");
            AddForeignKey("dbo.PaymentTransactions", "InvoiceId", "dbo.Invoices", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropForeignKey("dbo.PaymentTransactions", "InvoiceId", "dbo.Invoices");
            DropIndex("dbo.PaymentTransactions", new[] { "InvoiceId" });
            DropPrimaryKey("dbo.PaymentTransactions");
            AlterColumn("dbo.PaymentTransactions", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.PaymentTransactions", "InvoiceId");
            AddPrimaryKey("dbo.PaymentTransactions", "Id");
            CreateIndex("dbo.PaymentTransactions", "ApplicationId");
            AddForeignKey("dbo.PaymentTransactionTrans", "PaymentTransactionId", "dbo.PaymentTransactions", "Id");
            AddForeignKey("dbo.PaymentTransactions", "ApplicationId", "dbo.Applications", "Id");
        }
    }
}
