namespace GwcltdApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Production", "DailyActual", c => c.Double(nullable: false));
            AlterColumn("dbo.Production", "FRPH", c => c.Double(nullable: false));
            AlterColumn("dbo.Production", "FRPS", c => c.Double(nullable: false));
            AlterColumn("dbo.Production", "TFPD", c => c.Double(nullable: false));
            AlterColumn("dbo.Production", "NTFPD", c => c.Double(nullable: false));
            AlterColumn("dbo.Production", "LOG", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Production", "LOG", c => c.Int(nullable: false));
            AlterColumn("dbo.Production", "NTFPD", c => c.Int(nullable: false));
            AlterColumn("dbo.Production", "TFPD", c => c.Int(nullable: false));
            AlterColumn("dbo.Production", "FRPS", c => c.Int(nullable: false));
            AlterColumn("dbo.Production", "FRPH", c => c.Int(nullable: false));
            AlterColumn("dbo.Production", "DailyActual", c => c.Int(nullable: false));
        }
    }
}
