namespace GwcltdApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Production", "LOG", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Production", "LOG");
        }
    }
}
