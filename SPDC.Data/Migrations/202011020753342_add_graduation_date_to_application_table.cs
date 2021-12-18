namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_graduation_date_to_application_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "GraduationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "GraduationDate");
        }
    }
}
