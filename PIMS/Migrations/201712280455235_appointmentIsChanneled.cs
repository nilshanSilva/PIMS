namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentIsChanneled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "IsChanneled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "IsChanneled");
        }
    }
}
