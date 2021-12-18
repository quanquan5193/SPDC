namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTargetClassKey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TargetClasses", "CourseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TargetClasses", "CourseId", c => c.Int(nullable: false));
        }
    }
}
