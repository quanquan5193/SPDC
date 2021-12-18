namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldtoPaymenTransactionDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTransactionDocuments", "TypeOfDocument", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTransactionDocuments", "TypeOfDocument");
        }
    }
}
