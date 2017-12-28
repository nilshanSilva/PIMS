namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymenton : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        Cashier_Id = c.String(maxLength: 128),
                        Prescription_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Cashier_Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Cashier_Id)
                .Index(t => t.Prescription_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Payments", "Cashier_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "Prescription_Id" });
            DropIndex("dbo.Payments", new[] { "Cashier_Id" });
            DropTable("dbo.Payments");
        }
    }
}
