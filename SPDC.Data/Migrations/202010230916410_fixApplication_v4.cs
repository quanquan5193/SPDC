namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixApplication_v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "ExamVenueText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "ExamVenueText");
        }
    }
}
