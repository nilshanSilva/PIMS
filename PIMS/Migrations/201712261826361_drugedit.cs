namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drugedit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drugs", "Description", c => c.String());
            AddColumn("dbo.Drugs", "Category", c => c.Int(nullable: false));
            AddColumn("dbo.Drugs", "ReOrderLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Drugs", "Shelf", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drugs", "Shelf");
            DropColumn("dbo.Drugs", "ReOrderLevel");
            DropColumn("dbo.Drugs", "Category");
            DropColumn("dbo.Drugs", "Description");
        }
    }
}
