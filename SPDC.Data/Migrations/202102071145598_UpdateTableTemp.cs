namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableTemp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDocumentTemp", "DocumentId", "dbo.Documents");
            DropIndex("dbo.UserDocumentTemp", new[] { "DocumentId" });
            RenameColumn(table: "dbo.UserDocumentTemp", name: "DocumentId", newName: "Document_Id");
            AlterColumn("dbo.UserDocumentTemp", "Document_Id", c => c.Int());
            CreateIndex("dbo.UserDocumentTemp", "Document_Id");
            AddForeignKey("dbo.UserDocumentTemp", "Document_Id", "dbo.Documents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDocumentTemp", "Document_Id", "dbo.Documents");
            DropIndex("dbo.UserDocumentTemp", new[] { "Document_Id" });
            AlterColumn("dbo.UserDocumentTemp", "Document_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.UserDocumentTemp", name: "Document_Id", newName: "DocumentId");
            CreateIndex("dbo.UserDocumentTemp", "DocumentId");
            AddForeignKey("dbo.UserDocumentTemp", "DocumentId", "dbo.Documents", "Id", cascadeDelete: true);
        }
    }
}
