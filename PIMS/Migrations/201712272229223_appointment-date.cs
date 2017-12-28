namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "IsPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "IsPaid");
            DropColumn("dbo.Appointments", "CreatedDate");
        }
    }
}
