namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class presc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssuedDate = c.DateTime(nullable: false),
                        Disease = c.String(),
                        Description = c.String(),
                        AppointmentReference = c.Int(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        IsCleared = c.Boolean(nullable: false),
                        Issuer_Id = c.String(maxLength: 128),
                        Patient_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Issuer_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Patient_Id)
                .Index(t => t.Issuer_Id)
                .Index(t => t.Patient_Id);
            
            AddColumn("dbo.Drugs", "Prescription_Id", c => c.Int());
            CreateIndex("dbo.Drugs", "Prescription_Id");
            AddForeignKey("dbo.Drugs", "Prescription_Id", "dbo.Prescriptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "Patient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Prescriptions", "Issuer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Drugs", "Prescription_Id", "dbo.Prescriptions");
            DropIndex("dbo.Prescriptions", new[] { "Patient_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Issuer_Id" });
            DropIndex("dbo.Drugs", new[] { "Prescription_Id" });
            DropColumn("dbo.Drugs", "Prescription_Id");
            DropTable("dbo.Prescriptions");
        }
    }
}
