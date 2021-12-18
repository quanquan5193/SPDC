namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_exam_tran : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExamTrans", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ExamTrans", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.Exams", "ExamVenue", "dbo.CourseLocation");
            DropIndex("dbo.Exams", new[] { "ExamVenue" });
            DropIndex("dbo.ExamTrans", new[] { "LanguageId" });
            DropIndex("dbo.ExamTrans", new[] { "ExamId" });
            AddColumn("dbo.Exams", "CourseLocation_Id", c => c.Int());
            AlterColumn("dbo.Exams", "ExamVenue", c => c.String());
            CreateIndex("dbo.Exams", "CourseLocation_Id");
            AddForeignKey("dbo.Exams", "CourseLocation_Id", "dbo.CourseLocation", "Id");
            DropTable("dbo.ExamTrans");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExamTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        ExamId = c.Int(nullable: false),
                        Result = c.String(maxLength: 256),
                        Remarks = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Exams", "CourseLocation_Id", "dbo.CourseLocation");
            DropIndex("dbo.Exams", new[] { "CourseLocation_Id" });
            AlterColumn("dbo.Exams", "ExamVenue", c => c.Int(nullable: false));
            DropColumn("dbo.Exams", "CourseLocation_Id");
            CreateIndex("dbo.ExamTrans", "ExamId");
            CreateIndex("dbo.ExamTrans", "LanguageId");
            CreateIndex("dbo.Exams", "ExamVenue");
            AddForeignKey("dbo.Exams", "ExamVenue", "dbo.CourseLocation", "Id");
            AddForeignKey("dbo.ExamTrans", "ExamId", "dbo.Exams", "Id");
            AddForeignKey("dbo.ExamTrans", "LanguageId", "dbo.Languages", "Id");
        }
    }
}
