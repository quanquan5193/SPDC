namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefieldcoursedocumentation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CourseDocuments", "DistinguishDocType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourseDocuments", "DistinguishDocType", c => c.Boolean(nullable: false));
        }
    }
}
