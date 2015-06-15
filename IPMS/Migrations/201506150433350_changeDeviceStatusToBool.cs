namespace IdentitySample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDeviceStatusToBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "IsGood", c => c.Boolean(nullable: false));
            DropColumn("dbo.Devices", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Devices", "Status", c => c.String(nullable: false));
            DropColumn("dbo.Devices", "IsGood");
        }
    }
}
