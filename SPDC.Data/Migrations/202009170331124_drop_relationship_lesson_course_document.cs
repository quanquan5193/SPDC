namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop_relationship_lesson_course_document : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourseDocuments", "LessonId", "dbo.Lessons");
            DropIndex("dbo.CourseDocuments", new[] { "LessonId" });
            DropColumn("dbo.CourseDocuments", "LessonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourseDocuments", "LessonId", c => c.Int(nullable: false));
            CreateIndex("dbo.CourseDocuments", "LessonId");
            AddForeignKey("dbo.CourseDocuments", "LessonId", "dbo.Lessons", "Id");
        }
    }
}
