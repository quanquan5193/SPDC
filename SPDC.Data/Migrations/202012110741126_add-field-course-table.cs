namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldcoursetable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "ObjectiveEN", c => c.String());
            AddColumn("dbo.Courses", "ObjectiveTC", c => c.String());
            AddColumn("dbo.Courses", "ObjectiveSC", c => c.String());
            AddColumn("dbo.Courses", "WaitingTimeEN", c => c.String());
            AddColumn("dbo.Courses", "WaitingTimeTC", c => c.String());
            AddColumn("dbo.Courses", "WaitingTimeSC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "WaitingTimeSC");
            DropColumn("dbo.Courses", "WaitingTimeTC");
            DropColumn("dbo.Courses", "WaitingTimeEN");
            DropColumn("dbo.Courses", "ObjectiveSC");
            DropColumn("dbo.Courses", "ObjectiveTC");
            DropColumn("dbo.Courses", "ObjectiveEN");
        }
    }
}
