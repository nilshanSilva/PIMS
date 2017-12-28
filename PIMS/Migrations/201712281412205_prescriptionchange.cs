namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prescriptionchange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Drugs", "Prescription_Id", "dbo.Prescriptions");
            DropIndex("dbo.Drugs", new[] { "Prescription_Id" });
            CreateTable(
                "dbo.DrugItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        OrderQuantity = c.Int(nullable: false),
                        Total = c.Double(nullable: false),
                        IsPackedToPrescrition = c.Boolean(nullable: false),
                        ReferenceNumber = c.Int(nullable: false),
                        Drug_Id = c.Int(),
                        Prescription_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drugs", t => t.Drug_Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Drug_Id)
                .Index(t => t.Prescription_Id);
            
            DropColumn("dbo.Drugs", "Prescription_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drugs", "Prescription_Id", c => c.Int());
            DropForeignKey("dbo.DrugItems", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.DrugItems", "Drug_Id", "dbo.Drugs");
            DropIndex("dbo.DrugItems", new[] { "Prescription_Id" });
            DropIndex("dbo.DrugItems", new[] { "Drug_Id" });
            DropTable("dbo.DrugItems");
            CreateIndex("dbo.Drugs", "Prescription_Id");
            AddForeignKey("dbo.Drugs", "Prescription_Id", "dbo.Prescriptions", "Id");
        }
    }
}
