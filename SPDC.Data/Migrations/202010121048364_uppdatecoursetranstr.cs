namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uppdatecoursetranstr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CourseTrans", "Curriculum", c => c.String());
            AlterColumn("dbo.CourseTrans", "ConditionsOfCertificate", c => c.String());
            AlterColumn("dbo.CourseTrans", "Recognition", c => c.String());
            AlterColumn("dbo.CourseTrans", "AdmissionRequirements", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourseTrans", "AdmissionRequirements", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CourseTrans", "Recognition", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CourseTrans", "ConditionsOfCertificate", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CourseTrans", "Curriculum", c => c.String(maxLength: 1000));
        }
    }
}
