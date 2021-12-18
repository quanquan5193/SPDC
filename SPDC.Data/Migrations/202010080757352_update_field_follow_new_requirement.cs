namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_field_follow_new_requirement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "Capacity", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "ClassNumber", c => c.Int(nullable: false));
            AddColumn("dbo.EmployerRecommendationTrans", "Position", c => c.String());
            AddColumn("dbo.EmployerRecommendationTrans", "Email", c => c.String());
            AddColumn("dbo.Particular", "SameAddress", c => c.Boolean(nullable: false));
            AddColumn("dbo.Particular", "ResidentialRegionEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialDistrictEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialStreetNumberEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialStreetRoadEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialEstateQuartersVillageEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialBuildingEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialFloorEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialRmFtUnitEN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialRegionCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialDistrictCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialStreetNumberCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialStreetRoadCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialEstateQuartersVillageCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialBuildingCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialFloorCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "ResidentialRmFtUnitCN", c => c.String(maxLength: 256));
            AddColumn("dbo.Particular", "IsPrimamy", c => c.Boolean(nullable: false));
            AddColumn("dbo.Particular", "IsSecondary", c => c.Boolean(nullable: false));
            AddColumn("dbo.Particular", "IsTechInst", c => c.Boolean(nullable: false));
            AddColumn("dbo.Particular", "IsUniversityCollege", c => c.Boolean(nullable: false));
            AddColumn("dbo.WorkExperienceTrans", "JobNature", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkExperienceTrans", "JobNature");
            DropColumn("dbo.Particular", "IsUniversityCollege");
            DropColumn("dbo.Particular", "IsTechInst");
            DropColumn("dbo.Particular", "IsSecondary");
            DropColumn("dbo.Particular", "IsPrimamy");
            DropColumn("dbo.Particular", "ResidentialRmFtUnitCN");
            DropColumn("dbo.Particular", "ResidentialFloorCN");
            DropColumn("dbo.Particular", "ResidentialBuildingCN");
            DropColumn("dbo.Particular", "ResidentialEstateQuartersVillageCN");
            DropColumn("dbo.Particular", "ResidentialStreetRoadCN");
            DropColumn("dbo.Particular", "ResidentialStreetNumberCN");
            DropColumn("dbo.Particular", "ResidentialDistrictCN");
            DropColumn("dbo.Particular", "ResidentialRegionCN");
            DropColumn("dbo.Particular", "ResidentialRmFtUnitEN");
            DropColumn("dbo.Particular", "ResidentialFloorEN");
            DropColumn("dbo.Particular", "ResidentialBuildingEN");
            DropColumn("dbo.Particular", "ResidentialEstateQuartersVillageEN");
            DropColumn("dbo.Particular", "ResidentialStreetRoadEN");
            DropColumn("dbo.Particular", "ResidentialStreetNumberEN");
            DropColumn("dbo.Particular", "ResidentialDistrictEN");
            DropColumn("dbo.Particular", "ResidentialRegionEN");
            DropColumn("dbo.Particular", "SameAddress");
            DropColumn("dbo.EmployerRecommendationTrans", "Email");
            DropColumn("dbo.EmployerRecommendationTrans", "Position");
            DropColumn("dbo.Classes", "ClassNumber");
            DropColumn("dbo.Classes", "Capacity");
        }
    }
}
