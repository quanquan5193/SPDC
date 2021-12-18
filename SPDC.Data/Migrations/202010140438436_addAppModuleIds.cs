namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAppModuleIds : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationSetups",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        ShowEducationLevel = c.Boolean(nullable: false),
                        ShowRecommendation = c.Boolean(nullable: false),
                        ShowDocument = c.Boolean(nullable: false),
                        ShowCitf = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.RelevantMemberships",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ShowMembershipTable = c.Boolean(nullable: false),
                        ShowTwoYears = c.Boolean(nullable: false),
                        ShowKnowledge = c.Boolean(nullable: false),
                        ShowBimBasic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationSetups", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.RelevantWorks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ShowTwoYears = c.Boolean(nullable: false),
                        ShowThreeYears = c.Boolean(nullable: false),
                        ShowFourYears = c.Boolean(nullable: false),
                        ShowFiveYears = c.Boolean(nullable: false),
                        ShowThreeYearsLeak = c.Boolean(nullable: false),
                        ShowWorkingExperience = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationSetups", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Applications", "ModuleIds", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationSetups", "Id", "dbo.Courses");
            DropForeignKey("dbo.RelevantWorks", "Id", "dbo.ApplicationSetups");
            DropForeignKey("dbo.RelevantMemberships", "Id", "dbo.ApplicationSetups");
            DropIndex("dbo.RelevantWorks", new[] { "Id" });
            DropIndex("dbo.RelevantMemberships", new[] { "Id" });
            DropIndex("dbo.ApplicationSetups", new[] { "Id" });
            DropColumn("dbo.Applications", "ModuleIds");
            DropTable("dbo.RelevantWorks");
            DropTable("dbo.RelevantMemberships");
            DropTable("dbo.ApplicationSetups");
        }
    }
}
