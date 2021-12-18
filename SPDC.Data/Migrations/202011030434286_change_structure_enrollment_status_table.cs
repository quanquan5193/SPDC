namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_structure_enrollment_status_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnrollmentStatus", "NameEN", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.EnrollmentStatus", "NameTC", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.EnrollmentStatus", "NameSC", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.EnrollmentStatus", "LanguageId");
            DropColumn("dbo.EnrollmentStatus", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EnrollmentStatus", "Name", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.EnrollmentStatus", "LanguageId", c => c.Int(nullable: false));
            DropColumn("dbo.EnrollmentStatus", "NameSC");
            DropColumn("dbo.EnrollmentStatus", "NameTC");
            DropColumn("dbo.EnrollmentStatus", "NameEN");
        }
    }
}
