namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_course_data_table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CourseFee", c => c.Decimal(nullable: false, precision: 17, scale: 1));
            AlterColumn("dbo.Courses", "Allowance", c => c.Decimal(precision: 17, scale: 1));
            AlterColumn("dbo.Courses", "DiscountFee", c => c.Decimal(precision: 17, scale: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "DiscountFee", c => c.Decimal(precision: 18, scale: 0));
            AlterColumn("dbo.Courses", "Allowance", c => c.Decimal(precision: 18, scale: 0));
            AlterColumn("dbo.Courses", "CourseFee", c => c.Decimal(nullable: false, precision: 18, scale: 0));
        }
    }
}
