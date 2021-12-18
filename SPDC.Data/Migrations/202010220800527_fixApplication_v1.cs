namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixApplication_v1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "CountReExam", c => c.Byte());
            AddColumn("dbo.Exams", "IsReExam", c => c.Boolean(nullable: false));
            AddColumn("dbo.Lessons", "Venue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lessons", "Venue");
            DropColumn("dbo.Exams", "IsReExam");
            DropColumn("dbo.Classes", "CountReExam");
        }
    }
}
