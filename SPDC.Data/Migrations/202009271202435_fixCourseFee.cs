namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixCourseFee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "FeeRemarks1", c => c.String(maxLength: 256));
            AddColumn("dbo.Courses", "FeeRemarks2", c => c.String(maxLength: 256));
            AddColumn("dbo.ModuleCombinations", "ModuleNos", c => c.String());
            DropColumn("dbo.Courses", "FeeRemarks");
            DropColumn("dbo.ModuleCombinations", "ModuleNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ModuleCombinations", "ModuleNo", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "FeeRemarks", c => c.String(maxLength: 256));
            DropColumn("dbo.ModuleCombinations", "ModuleNos");
            DropColumn("dbo.Courses", "FeeRemarks2");
            DropColumn("dbo.Courses", "FeeRemarks1");
        }
    }
}
