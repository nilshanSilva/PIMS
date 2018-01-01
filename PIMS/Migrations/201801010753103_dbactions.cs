namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionType = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                        ActionedQuantity = c.Int(nullable: false),
                        ActionedDate = c.Int(nullable: false),
                        Doctor_Id = c.String(maxLength: 128),
                        Drug_Id = c.Int(),
                        Patient_Id = c.String(maxLength: 128),
                        Pharmacist_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Doctor_Id)
                .ForeignKey("dbo.Drugs", t => t.Drug_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Patient_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Pharmacist_Id)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Drug_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.Pharmacist_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Actions", "Pharmacist_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Actions", "Patient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Actions", "Drug_Id", "dbo.Drugs");
            DropForeignKey("dbo.Actions", "Doctor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Actions", new[] { "Pharmacist_Id" });
            DropIndex("dbo.Actions", new[] { "Patient_Id" });
            DropIndex("dbo.Actions", new[] { "Drug_Id" });
            DropIndex("dbo.Actions", new[] { "Doctor_Id" });
            DropTable("dbo.Actions");
        }
    }
}
