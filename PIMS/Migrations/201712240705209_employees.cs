namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ContactNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "NIC", c => c.String());
            AddColumn("dbo.AspNetUsers", "JoinedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Specialization", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Qualification", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ChannelFee", c => c.Double());
            AddColumn("dbo.AspNetUsers", "ChannelStartTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "AverageChannelDuration", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ChannelEndTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "NumOfPatientsPerDay", c => c.Int());
            AddColumn("dbo.AspNetUsers", "City", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserLevel", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropColumn("dbo.AspNetUsers", "UserLevel");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "NumOfPatientsPerDay");
            DropColumn("dbo.AspNetUsers", "ChannelEndTime");
            DropColumn("dbo.AspNetUsers", "AverageChannelDuration");
            DropColumn("dbo.AspNetUsers", "ChannelStartTime");
            DropColumn("dbo.AspNetUsers", "ChannelFee");
            DropColumn("dbo.AspNetUsers", "Qualification");
            DropColumn("dbo.AspNetUsers", "Specialization");
            DropColumn("dbo.AspNetUsers", "JoinedAt");
            DropColumn("dbo.AspNetUsers", "NIC");
            DropColumn("dbo.AspNetUsers", "ContactNumber");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "BirthDate");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
