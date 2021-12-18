namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentTransactionDocuments", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropPrimaryKey("dbo.PaymentTransactions");
            AlterColumn("dbo.PaymentTransactions", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.PaymentTransactions", "Id");
            AddForeignKey("dbo.PaymentTransactionDocuments", "PaymentTransactionId", "dbo.PaymentTransactions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentTransactionDocuments", "PaymentTransactionId", "dbo.PaymentTransactions");
            DropPrimaryKey("dbo.PaymentTransactions");
            AlterColumn("dbo.PaymentTransactions", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PaymentTransactions", "Id");
            AddForeignKey("dbo.PaymentTransactionDocuments", "PaymentTransactionId", "dbo.PaymentTransactions", "Id");
        }
    }
}
