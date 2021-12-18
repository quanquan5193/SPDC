namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditWorkExperienceTemp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkExperienceTrans", "Position", c => c.String(maxLength: 256));
            AlterColumn("dbo.WorkExperienceTempTran", "Position", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkExperienceTempTran", "Position", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.WorkExperienceTrans", "Position", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
