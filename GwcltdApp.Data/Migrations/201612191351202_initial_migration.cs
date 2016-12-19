namespace GwcltdApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Error",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Option",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        OptionOf = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OptionType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Production",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DayToRecord = c.DateTime(nullable: false),
                        DailyActual = c.Int(nullable: false),
                        Comment = c.String(maxLength: 2000),
                        FRPH = c.Int(nullable: false),
                        FRPS = c.Int(nullable: false),
                        TFPD = c.Int(nullable: false),
                        NTFPD = c.Int(nullable: false),
                        LOG = c.Int(nullable: false),
                        WSystemId = c.Int(nullable: false),
                        OptionId = c.Int(nullable: false),
                        OptionTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Option", t => t.OptionId, cascadeDelete: true)
                .ForeignKey("dbo.OptionType", t => t.OptionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.WSystem", t => t.WSystemId, cascadeDelete: true)
                .Index(t => t.WSystemId)
                .Index(t => t.OptionId)
                .Index(t => t.OptionTypeId);
            
            CreateTable(
                "dbo.WSystem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 20),
                        Capacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SMS_IN",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SMS_TEXT = c.String(),
                        SENDER_NUMBER = c.String(),
                        SENT_DT = c.DateTime(nullable: false),
                        TS = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 200),
                        HashedPassword = c.String(nullable: false, maxLength: 200),
                        Salt = c.String(nullable: false, maxLength: 200),
                        IsLocked = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Production", "WSystemId", "dbo.WSystem");
            DropForeignKey("dbo.Production", "OptionTypeId", "dbo.OptionType");
            DropForeignKey("dbo.Production", "OptionId", "dbo.Option");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.Production", new[] { "OptionTypeId" });
            DropIndex("dbo.Production", new[] { "OptionId" });
            DropIndex("dbo.Production", new[] { "WSystemId" });
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.SMS_IN");
            DropTable("dbo.Role");
            DropTable("dbo.WSystem");
            DropTable("dbo.Production");
            DropTable("dbo.OptionType");
            DropTable("dbo.Option");
            DropTable("dbo.Error");
        }
    }
}
