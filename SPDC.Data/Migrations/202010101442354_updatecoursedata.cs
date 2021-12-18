namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecoursedata : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lecturers", "UserId", "dbo.Users");
            DropForeignKey("dbo.LocalizationLabels", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Lecturers", "CourseId", "dbo.Courses");
            DropIndex("dbo.Lecturers", new[] { "CourseId" });
            DropIndex("dbo.Lecturers", new[] { "UserId" });
            DropIndex("dbo.LocalizationLabels", new[] { "LanguageId" });
            CreateTable(
                "dbo.LevelofApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NameEN = c.String(),
                        NameCN = c.String(),
                        NameHK = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MediumOfInstructions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NameEN = c.String(),
                        NameCN = c.String(),
                        NameHK = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgrammeLeaders",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NameEN = c.String(),
                        NameCN = c.String(),
                        NameHK = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Courses", "LecturerId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "MediumOfInstructionId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "LevelOfApprovalId", c => c.Int(nullable: false));
            AddColumn("dbo.Lecturers", "NameEN", c => c.String());
            AddColumn("dbo.Lecturers", "NameCN", c => c.String());
            AddColumn("dbo.Lecturers", "NameHK", c => c.String());
            CreateIndex("dbo.Courses", "ProgrammeLeaderId");
            CreateIndex("dbo.Courses", "LecturerId");
            CreateIndex("dbo.Courses", "MediumOfInstructionId");
            CreateIndex("dbo.Courses", "LevelOfApprovalId");
            AddForeignKey("dbo.Courses", "LecturerId", "dbo.Lecturers", "Id");
            AddForeignKey("dbo.Courses", "LevelOfApprovalId", "dbo.LevelofApprovals", "Id");
            AddForeignKey("dbo.Courses", "MediumOfInstructionId", "dbo.MediumOfInstructions", "Id");
            AddForeignKey("dbo.Courses", "ProgrammeLeaderId", "dbo.ProgrammeLeaders", "Id");
            DropColumn("dbo.Courses", "MediumOfInstruction");
            DropColumn("dbo.Courses", "LevelOfApproval");
            DropColumn("dbo.Lecturers", "CourseId");
            DropColumn("dbo.Lecturers", "UserId");
            DropTable("dbo.LocalizationLabels");
            DropTable("dbo.CourseMasterDatas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CourseMasterDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocalizationLabels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Key = c.String(nullable: false, maxLength: 256, unicode: false),
                        Value = c.String(maxLength: 256),
                        FormName = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Lecturers", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Lecturers", "CourseId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "LevelOfApproval", c => c.String(maxLength: 256));
            AddColumn("dbo.Courses", "MediumOfInstruction", c => c.Int(nullable: false));
            DropForeignKey("dbo.Courses", "ProgrammeLeaderId", "dbo.ProgrammeLeaders");
            DropForeignKey("dbo.Courses", "MediumOfInstructionId", "dbo.MediumOfInstructions");
            DropForeignKey("dbo.Courses", "LevelOfApprovalId", "dbo.LevelofApprovals");
            DropForeignKey("dbo.Courses", "LecturerId", "dbo.Lecturers");
            DropIndex("dbo.Courses", new[] { "LevelOfApprovalId" });
            DropIndex("dbo.Courses", new[] { "MediumOfInstructionId" });
            DropIndex("dbo.Courses", new[] { "LecturerId" });
            DropIndex("dbo.Courses", new[] { "ProgrammeLeaderId" });
            DropColumn("dbo.Lecturers", "NameHK");
            DropColumn("dbo.Lecturers", "NameCN");
            DropColumn("dbo.Lecturers", "NameEN");
            DropColumn("dbo.Courses", "LevelOfApprovalId");
            DropColumn("dbo.Courses", "MediumOfInstructionId");
            DropColumn("dbo.Courses", "LecturerId");
            DropTable("dbo.ProgrammeLeaders");
            DropTable("dbo.MediumOfInstructions");
            DropTable("dbo.LevelofApprovals");
            CreateIndex("dbo.LocalizationLabels", "LanguageId");
            CreateIndex("dbo.Lecturers", "UserId");
            CreateIndex("dbo.Lecturers", "CourseId");
            AddForeignKey("dbo.Lecturers", "CourseId", "dbo.Courses", "Id");
            AddForeignKey("dbo.LocalizationLabels", "LanguageId", "dbo.Languages", "Id");
            AddForeignKey("dbo.Lecturers", "UserId", "dbo.Users", "Id");
        }
    }
}
