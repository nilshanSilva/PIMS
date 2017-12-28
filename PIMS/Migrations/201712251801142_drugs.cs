namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drugs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drugs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenericName = c.String(nullable: false),
                        BrandName = c.String(nullable: false),
                        NetContent = c.String(nullable: false),
                        DrugForm = c.Int(nullable: false),
                        RecievedPrice = c.Double(nullable: false),
                        RetailPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        SupplierName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Drugs");
        }
    }
}
