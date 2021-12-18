namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldCheckReExamForInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "TypeReExam", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "TypeReExam");
        }
    }
}
