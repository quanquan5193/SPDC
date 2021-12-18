namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixApplication_v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "ModuleId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "ModuleId");
        }
    }
}
