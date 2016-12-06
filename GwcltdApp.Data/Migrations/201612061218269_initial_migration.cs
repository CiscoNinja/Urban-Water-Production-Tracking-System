namespace GwcltdApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Production", "FRPH", c => c.Int(nullable: false));
            AddColumn("dbo.Production", "FRPS", c => c.Int(nullable: false));
            AddColumn("dbo.Production", "TFPD", c => c.Int(nullable: false));
            AddColumn("dbo.Production", "NTFPD", c => c.Int(nullable: false));
            AddColumn("dbo.WSystem", "Capacity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WSystem", "Capacity");
            DropColumn("dbo.Production", "NTFPD");
            DropColumn("dbo.Production", "TFPD");
            DropColumn("dbo.Production", "FRPS");
            DropColumn("dbo.Production", "FRPH");
        }
    }
}
