namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeunnecessaryfield : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "DateCreated");
            DropColumn("dbo.Courses", "CreatedBy");
            DropColumn("dbo.Courses", "DateModified");
            DropColumn("dbo.Courses", "UpdatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "UpdatedBy", c => c.Int());
            AddColumn("dbo.Courses", "DateModified", c => c.DateTime());
            AddColumn("dbo.Courses", "CreatedBy", c => c.Int());
            AddColumn("dbo.Courses", "DateCreated", c => c.DateTime());
        }
    }
}
