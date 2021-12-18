namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_axem_attendance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MakeUpClasses", "TimeFromHrs", c => c.Int(nullable: false));
            AddColumn("dbo.MakeUpClasses", "TimeToHrs", c => c.Int(nullable: false));
            AddColumn("dbo.ResitExames", "TimeFromHrs", c => c.Int(nullable: false));
            AddColumn("dbo.ResitExames", "TimeToHrs", c => c.Int(nullable: false));
            DropColumn("dbo.MakeUpClasses", "TimeFromSec");
            DropColumn("dbo.MakeUpClasses", "TimeToSec");
            DropColumn("dbo.ResitExames", "TimeFromSec");
            DropColumn("dbo.ResitExames", "TimeToSec");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResitExames", "TimeToSec", c => c.Int(nullable: false));
            AddColumn("dbo.ResitExames", "TimeFromSec", c => c.Int(nullable: false));
            AddColumn("dbo.MakeUpClasses", "TimeToSec", c => c.Int(nullable: false));
            AddColumn("dbo.MakeUpClasses", "TimeFromSec", c => c.Int(nullable: false));
            DropColumn("dbo.ResitExames", "TimeToHrs");
            DropColumn("dbo.ResitExames", "TimeFromHrs");
            DropColumn("dbo.MakeUpClasses", "TimeToHrs");
            DropColumn("dbo.MakeUpClasses", "TimeFromHrs");
        }
    }
}
