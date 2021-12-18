namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "ClassCommonId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "ClassCommonId");
        }
    }
}
