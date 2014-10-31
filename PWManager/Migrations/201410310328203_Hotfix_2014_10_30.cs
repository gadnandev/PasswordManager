namespace PWManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Hotfix_2014_10_30 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "LoginName", c => c.String(nullable: false, maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "LoginName", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
