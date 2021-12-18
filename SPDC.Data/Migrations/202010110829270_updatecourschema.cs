namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecourschema : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Courses", new[] { "ProgrammeLeaderId" });
            DropIndex("dbo.Courses", new[] { "LecturerId" });
            DropIndex("dbo.Courses", new[] { "MediumOfInstructionId" });
            DropIndex("dbo.Courses", new[] { "LevelOfApprovalId" });
            AlterColumn("dbo.Courses", "ProgrammeLeaderId", c => c.Int());
            AlterColumn("dbo.Courses", "LecturerId", c => c.Int());
            AlterColumn("dbo.Courses", "MediumOfInstructionId", c => c.Int());
            AlterColumn("dbo.Courses", "LevelOfApprovalId", c => c.Int());
            AlterColumn("dbo.CourseTrans", "Curriculum", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CourseTrans", "ConditionsOfCertificate", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CourseTrans", "Recognition", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CourseTrans", "AdmissionRequirements", c => c.String(maxLength: 1000));
            CreateIndex("dbo.Courses", "ProgrammeLeaderId");
            CreateIndex("dbo.Courses", "LecturerId");
            CreateIndex("dbo.Courses", "MediumOfInstructionId");
            CreateIndex("dbo.Courses", "LevelOfApprovalId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Courses", new[] { "LevelOfApprovalId" });
            DropIndex("dbo.Courses", new[] { "MediumOfInstructionId" });
            DropIndex("dbo.Courses", new[] { "LecturerId" });
            DropIndex("dbo.Courses", new[] { "ProgrammeLeaderId" });
            AlterColumn("dbo.CourseTrans", "AdmissionRequirements", c => c.String(maxLength: 256));
            AlterColumn("dbo.CourseTrans", "Recognition", c => c.String(maxLength: 256));
            AlterColumn("dbo.CourseTrans", "ConditionsOfCertificate", c => c.String(maxLength: 256));
            AlterColumn("dbo.CourseTrans", "Curriculum", c => c.String(maxLength: 256));
            AlterColumn("dbo.Courses", "LevelOfApprovalId", c => c.Int(nullable: false));
            AlterColumn("dbo.Courses", "MediumOfInstructionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Courses", "LecturerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Courses", "ProgrammeLeaderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "LevelOfApprovalId");
            CreateIndex("dbo.Courses", "MediumOfInstructionId");
            CreateIndex("dbo.Courses", "LecturerId");
            CreateIndex("dbo.Courses", "ProgrammeLeaderId");
        }
    }
}
