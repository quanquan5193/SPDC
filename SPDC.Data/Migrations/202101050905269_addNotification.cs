namespace SPDC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationUser",
                c => new
                    {
                        NotificationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsFavourite = c.Boolean(nullable: false),
                        IsRemove = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationId, t.UserId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        Type = c.Int(nullable: false),
                        DataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDevices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceToken = c.String(maxLength: 256),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.DeviceToken, unique: true, name: "INDEX_DEVICETOKEN")
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDevices", "UserId", "dbo.Users");
            DropForeignKey("dbo.NotificationUser", "UserId", "dbo.Users");
            DropForeignKey("dbo.NotificationUser", "NotificationId", "dbo.Notifications");
            DropIndex("dbo.UserDevices", new[] { "UserId" });
            DropIndex("dbo.UserDevices", "INDEX_DEVICETOKEN");
            DropIndex("dbo.NotificationUser", new[] { "UserId" });
            DropIndex("dbo.NotificationUser", new[] { "NotificationId" });
            DropTable("dbo.UserDevices");
            DropTable("dbo.Notifications");
            DropTable("dbo.NotificationUser");
        }
    }
}
