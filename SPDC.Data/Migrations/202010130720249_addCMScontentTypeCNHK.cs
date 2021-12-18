namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCMScontentTypeCNHK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsContentTypes", "NameTC", c => c.String(maxLength: 256));
            AddColumn("dbo.CmsContentTypes", "NameSC", c => c.String(maxLength: 256));
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
            DropColumn("dbo.CmsContentTypes", "NameSC");
            DropColumn("dbo.CmsContentTypes", "NameTC");
        }
    }
}
