namespace IdentitySample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notifications", "SeenTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "SeenTime", c => c.DateTime(nullable: false));
        }
    }
}
