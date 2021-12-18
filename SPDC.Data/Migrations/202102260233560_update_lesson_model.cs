namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_lesson_model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResitExamApplications", "AssessmentMark", c => c.Int());
            AddColumn("dbo.ResitExamApplications", "AssessmentResult", c => c.Int());
            AddColumn("dbo.Lessons", "TimeFromMin", c => c.Int(nullable: false));
            AddColumn("dbo.Lessons", "TimeFromHrs", c => c.Int(nullable: false));
            AddColumn("dbo.Lessons", "TimeToMin", c => c.Int(nullable: false));
            AddColumn("dbo.Lessons", "TimeToHrs", c => c.Int(nullable: false));
            AddColumn("dbo.LessonAttendances", "FromLessonId", c => c.Int(nullable: false));
            AddColumn("dbo.MakeUpAttendences", "IsTakeAttendance", c => c.Boolean(nullable: false));
            DropColumn("dbo.Lessons", "FromTime");
            DropColumn("dbo.Lessons", "ToTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lessons", "ToTime", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.Lessons", "FromTime", c => c.String(nullable: false, maxLength: 256, unicode: false));
            DropColumn("dbo.MakeUpAttendences", "IsTakeAttendance");
            DropColumn("dbo.LessonAttendances", "FromLessonId");
            DropColumn("dbo.Lessons", "TimeToHrs");
            DropColumn("dbo.Lessons", "TimeToMin");
            DropColumn("dbo.Lessons", "TimeFromHrs");
            DropColumn("dbo.Lessons", "TimeFromMin");
            DropColumn("dbo.ResitExamApplications", "AssessmentResult");
            DropColumn("dbo.ResitExamApplications", "AssessmentMark");
        }
    }
}
