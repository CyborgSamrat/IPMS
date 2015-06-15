namespace IdentitySample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allmostNothing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Complaints", "AssignedTo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Complaints", "AssignedTo", c => c.String(nullable: true));
        }
    }
}
