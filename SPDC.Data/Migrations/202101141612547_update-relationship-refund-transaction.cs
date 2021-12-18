namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterelationshiprefundtransaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RefundTransactionApprovedStatusHistories", "RefundTransactionId", "dbo.RefundTransactions");
            AddForeignKey("dbo.RefundTransactionApprovedStatusHistories", "RefundTransactionId", "dbo.RefundTransactions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefundTransactionApprovedStatusHistories", "RefundTransactionId", "dbo.RefundTransactions");
            AddForeignKey("dbo.RefundTransactionApprovedStatusHistories", "RefundTransactionId", "dbo.RefundTransactions", "Id", cascadeDelete: true);
        }
    }
}
