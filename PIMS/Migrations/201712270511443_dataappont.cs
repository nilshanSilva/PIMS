namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataappont : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "Patient_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "Doctor_Id" });
            DropIndex("dbo.Appointments", new[] { "Patient_Id" });
            AddColumn("dbo.Appointments", "PatientId", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "DoctorId", c => c.String(nullable: false));
            DropColumn("dbo.Appointments", "Doctor_Id");
            DropColumn("dbo.Appointments", "Patient_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "Patient_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Appointments", "Doctor_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Appointments", "DoctorId");
            DropColumn("dbo.Appointments", "PatientId");
            CreateIndex("dbo.Appointments", "Patient_Id");
            CreateIndex("dbo.Appointments", "Doctor_Id");
            AddForeignKey("dbo.Appointments", "Patient_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
