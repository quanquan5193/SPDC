namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtablesubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSubscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company = c.String(nullable: false, maxLength: 256),
                        Honorific = c.Int(nullable: false),
                        FirstNameEN = c.String(nullable: false, maxLength: 256),
                        LastNameEN = c.String(nullable: false, maxLength: 256),
                        FirstNameCN = c.String(nullable: false, maxLength: 256),
                        LastNameCN = c.String(nullable: false, maxLength: 256),
                        PrefixMobilePhone = c.String(maxLength: 10),
                        MobilePhone = c.String(maxLength: 50),
                        Position = c.String(maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 256),
                        InterestedTypeOfCourse = c.String(nullable: false, maxLength: 256),
                        CommunicationLanguage = c.Int(nullable: false),
                        IsSubscribe = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "INDEX_Email");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserSubscriptions", "INDEX_Email");
            DropTable("dbo.UserSubscriptions");
        }
    }
}
