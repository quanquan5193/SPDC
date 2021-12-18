namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateinvoiceitemandinvoicetable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "InvoiceType", "dbo.InvoiceTypes");
            DropIndex("dbo.Invoices", new[] { "InvoiceType" });
            CreateTable(
                "dbo.InvoiceItemTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Invoices", "RequiresHardCopyReceipt", c => c.Boolean(nullable: false));
            AddColumn("dbo.Invoices", "PaymentDueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.InvoiceItems", "EnglishName", c => c.String());
            AddColumn("dbo.InvoiceItems", "ChineseName", c => c.String());
            AddColumn("dbo.InvoiceItems", "IsDiscount", c => c.Boolean(nullable: false));
            AddColumn("dbo.InvoiceItems", "InvoiceItemTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.InvoiceItems", "InvoiceItemTypeId");
            AddForeignKey("dbo.InvoiceItems", "InvoiceItemTypeId", "dbo.InvoiceItemTypes", "Id");
            DropColumn("dbo.Invoices", "InvoiceType");
            DropTable("dbo.InvoiceTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InvoiceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Invoices", "InvoiceType", c => c.Int(nullable: false));
            DropForeignKey("dbo.InvoiceItems", "InvoiceItemTypeId", "dbo.InvoiceItemTypes");
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceItemTypeId" });
            DropColumn("dbo.InvoiceItems", "InvoiceItemTypeId");
            DropColumn("dbo.InvoiceItems", "IsDiscount");
            DropColumn("dbo.InvoiceItems", "ChineseName");
            DropColumn("dbo.InvoiceItems", "EnglishName");
            DropColumn("dbo.Invoices", "PaymentDueDate");
            DropColumn("dbo.Invoices", "RequiresHardCopyReceipt");
            DropTable("dbo.InvoiceItemTypes");
            CreateIndex("dbo.Invoices", "InvoiceType");
            AddForeignKey("dbo.Invoices", "InvoiceType", "dbo.InvoiceTypes", "Id");
        }
    }
}
