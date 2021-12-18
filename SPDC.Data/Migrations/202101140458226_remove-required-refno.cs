namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerequiredrefno : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentTransactions", "RefNo", c => c.String(maxLength: 265, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentTransactions", "RefNo", c => c.String(nullable: false, maxLength: 265, unicode: false));
        }
    }
}
