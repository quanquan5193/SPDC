namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldbatchpaymentamount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchPayments", "BatchPaymentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchPayments", "BatchPaymentAmount");
        }
    }
}
