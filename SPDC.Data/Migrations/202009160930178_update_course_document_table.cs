namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_course_document_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseDocuments", "DistinguishDocType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseDocuments", "DistinguishDocType");
        }
    }
}
