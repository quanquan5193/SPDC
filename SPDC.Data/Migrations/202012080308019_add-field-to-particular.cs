namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldtoparticular : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Particular", "InterestedTypeOfCourse", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Particular", "InterestedTypeOfCourse");
        }
    }
}
