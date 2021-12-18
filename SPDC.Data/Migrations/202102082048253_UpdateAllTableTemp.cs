namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAllTableTemp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EmployerRecommendationTemp", "ApplicationId");
            DropColumn("dbo.QualificationTemp", "ApplicationId");
            DropColumn("dbo.UserDocumentTemp", "ApplicationId");
            DropColumn("dbo.ParticularTemp", "ApplicationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParticularTemp", "ApplicationId", c => c.Int(nullable: false));
            AddColumn("dbo.UserDocumentTemp", "ApplicationId", c => c.Int(nullable: false));
            AddColumn("dbo.QualificationTemp", "ApplicationId", c => c.Int(nullable: false));
            AddColumn("dbo.EmployerRecommendationTemp", "ApplicationId", c => c.Int(nullable: false));
        }
    }
}
