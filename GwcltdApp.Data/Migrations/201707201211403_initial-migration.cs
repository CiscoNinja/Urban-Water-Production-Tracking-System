namespace GwcltdApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WSystem", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.WSystem", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Production", "Comment", c => c.String());
            AlterColumn("dbo.Option", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.OptionType", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.User", "HashedPassword", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Salt", c => c.String(nullable: false));
            AlterColumn("dbo.Role", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Role", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Salt", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.User", "HashedPassword", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.User", "Username", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.OptionType", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Option", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Production", "Comment", c => c.String(maxLength: 2000));
            AlterColumn("dbo.WSystem", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.WSystem", "Code", c => c.String(nullable: false, maxLength: 5));
        }
    }
}
