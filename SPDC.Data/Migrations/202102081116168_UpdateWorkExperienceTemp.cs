namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkExperienceTemp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkExperienceTempTran", "LanguageId", "dbo.Languages");
            DropIndex("dbo.WorkExperienceTempTran", new[] { "LanguageId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.WorkExperienceTempTran", "LanguageId");
            AddForeignKey("dbo.WorkExperienceTempTran", "LanguageId", "dbo.Languages", "Id", cascadeDelete: true);
        }
    }
}
