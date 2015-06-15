namespace IdentitySample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        ComplaintId = c.Int(nullable: false, identity: true),
                        LodgedOn = c.DateTime(nullable: false),
                        IsSolved = c.Boolean(nullable: false),
                        IsAssigned = c.Boolean(nullable: false),
                        Status = c.String(),
                        Issue = c.String(nullable: false),
                        Description = c.String(),
                        LodgedBy = c.String(nullable: false),
                        AssignedTo = c.String(nullable: false),
                        Device_DeviceId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ComplaintId)
                .ForeignKey("dbo.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                        UserId = c.String(nullable: false),
                        Location_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .Index(t => t.Location_LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Room = c.Int(nullable: false),
                        Row = c.Int(nullable: false),
                        Column = c.Int(nullable: false),
                        Device = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackId = c.Int(nullable: false, identity: true),
                        Issue = c.String(nullable: false),
                        Body = c.String(),
                        GivenBy = c.String(nullable: false),
                        ComplaintId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FeedbackId)
                .ForeignKey("dbo.Complaints", t => t.ComplaintId, cascadeDelete: true)
                .Index(t => t.ComplaintId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        SendTime = c.DateTime(nullable: false),
                        SeenTime = c.DateTime(nullable: false),
                        Issue = c.String(),
                        IsSeen = c.Boolean(nullable: false),
                        Sender = c.String(nullable: false),
                        Receiver = c.String(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.AspNetUsers", "IpmsId", c => c.String());
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Device", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "profileImage", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Complaints", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "ComplaintId", "dbo.Complaints");
            DropForeignKey("dbo.Complaints", "Device_DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "Location_LocationId", "dbo.Locations");
            DropIndex("dbo.Notifications", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Feedbacks", new[] { "ComplaintId" });
            DropIndex("dbo.Devices", new[] { "Location_LocationId" });
            DropIndex("dbo.Complaints", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Complaints", new[] { "Device_DeviceId" });
            DropColumn("dbo.AspNetUsers", "profileImage");
            DropColumn("dbo.AspNetUsers", "Device");
            DropColumn("dbo.AspNetUsers", "FullName");
            DropColumn("dbo.AspNetUsers", "IpmsId");
            DropTable("dbo.Notifications");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Locations");
            DropTable("dbo.Devices");
            DropTable("dbo.Complaints");
        }
    }
}
