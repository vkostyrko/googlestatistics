namespace Admixer.AdEx.Integration.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdExGoogleStats",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        Id = c.Long(nullable: false),
                        Size = c.String(nullable: false, maxLength: 128),
                        TagName = c.String(),
                        MatchedRequests = c.Long(),
                        Clicks = c.Int(),
                        CTR = c.Decimal(precision: 18, scale: 2),
                        eCPM = c.Decimal(precision: 18, scale: 2),
                        Revenue = c.Decimal(precision: 18, scale: 2),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Requests = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.Id, t.Size });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdExGoogleStats");
        }
    }
}
