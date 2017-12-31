namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "Feedback", c => c.String());
            AddColumn("dbo.Prescriptions", "FeedbackSubmitted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescriptions", "FeedbackSubmitted");
            DropColumn("dbo.Prescriptions", "Feedback");
        }
    }
}
