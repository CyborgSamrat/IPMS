namespace IdentitySample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Devices", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Complaints", "Device_DeviceId", "dbo.Devices");
            DropIndex("dbo.Complaints", new[] { "Device_DeviceId" });
            DropIndex("dbo.Devices", new[] { "Location_LocationId" });
            AddColumn("dbo.Complaints", "DeviceId", c => c.Int(nullable: false));
            AddColumn("dbo.Devices", "LocationId", c => c.Int(nullable: false));
            DropColumn("dbo.Complaints", "Device_DeviceId");
            DropColumn("dbo.Devices", "Location_LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Devices", "Location_LocationId", c => c.Int(nullable: false));
            AddColumn("dbo.Complaints", "Device_DeviceId", c => c.Int(nullable: false));
            DropColumn("dbo.Devices", "LocationId");
            DropColumn("dbo.Complaints", "DeviceId");
            CreateIndex("dbo.Devices", "Location_LocationId");
            CreateIndex("dbo.Complaints", "Device_DeviceId");
            AddForeignKey("dbo.Complaints", "Device_DeviceId", "dbo.Devices", "DeviceId", cascadeDelete: true);
            AddForeignKey("dbo.Devices", "Location_LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
    }
}
