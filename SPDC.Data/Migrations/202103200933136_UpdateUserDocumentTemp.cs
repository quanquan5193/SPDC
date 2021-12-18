namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserDocumentTemp : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserDocumentTemp", new[] { "Document_Id" });
            RenameColumn(table: "dbo.UserDocumentTemp", name: "Document_Id", newName: "DocumentId");
            AlterColumn("dbo.UserDocumentTemp", "DocumentId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserDocumentTemp", "DocumentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserDocumentTemp", new[] { "DocumentId" });
            AlterColumn("dbo.UserDocumentTemp", "DocumentId", c => c.Int());
            RenameColumn(table: "dbo.UserDocumentTemp", name: "DocumentId", newName: "Document_Id");
            CreateIndex("dbo.UserDocumentTemp", "Document_Id");
        }
    }
}
