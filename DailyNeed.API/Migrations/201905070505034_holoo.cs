namespace DailyNeed.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holoo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FarmRegistrations", "Contact", c => c.String());
            AddColumn("dbo.FarmRegistrations", "PrimaryContactNumber", c => c.String());
            AddColumn("dbo.FarmRegistrations", "EmailId", c => c.String());
            AddColumn("dbo.FarmRegistrations", "Company_Id", c => c.Int());
            CreateIndex("dbo.FarmRegistrations", "Company_Id");
            AddForeignKey("dbo.FarmRegistrations", "Company_Id", "dbo.Companies", "Id");
            DropColumn("dbo.FarmRegistrations", "Name");
            DropColumn("dbo.FarmRegistrations", "Email");
            DropColumn("dbo.FarmRegistrations", "State");
            DropColumn("dbo.FarmRegistrations", "ContactNumber");
            DropColumn("dbo.FarmRegistrations", "Password");
            DropColumn("dbo.FarmRegistrations", "ConfirmPassword");
            DropColumn("dbo.People", "FarmName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "FarmName", c => c.String());
            AddColumn("dbo.FarmRegistrations", "ConfirmPassword", c => c.String());
            AddColumn("dbo.FarmRegistrations", "Password", c => c.String());
            AddColumn("dbo.FarmRegistrations", "ContactNumber", c => c.String());
            AddColumn("dbo.FarmRegistrations", "State", c => c.String());
            AddColumn("dbo.FarmRegistrations", "Email", c => c.String());
            AddColumn("dbo.FarmRegistrations", "Name", c => c.String());
            DropForeignKey("dbo.FarmRegistrations", "Company_Id", "dbo.Companies");
            DropIndex("dbo.FarmRegistrations", new[] { "Company_Id" });
            DropColumn("dbo.FarmRegistrations", "Company_Id");
            DropColumn("dbo.FarmRegistrations", "EmailId");
            DropColumn("dbo.FarmRegistrations", "PrimaryContactNumber");
            DropColumn("dbo.FarmRegistrations", "Contact");
        }
    }
}
