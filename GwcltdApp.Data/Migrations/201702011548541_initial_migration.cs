namespace GwcltdApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaRegion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GwclAreaId = c.Int(nullable: false),
                        GwclRegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclArea", t => t.GwclAreaId, cascadeDelete: false)
                .ForeignKey("dbo.GwclRegion", t => t.GwclRegionId, cascadeDelete: false)
                .Index(t => t.GwclAreaId)
                .Index(t => t.GwclRegionId);
            
            CreateTable(
                "dbo.GwclRegion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        GwclAreaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclArea", t => t.GwclAreaID, cascadeDelete: false)
                .Index(t => t.GwclAreaID);
            
            CreateTable(
                "dbo.GwclArea",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RegionStation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GwclRegionId = c.Int(nullable: false),
                        GwclStationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclStation", t => t.GwclStationId, cascadeDelete: false)
                .ForeignKey("dbo.GwclRegion", t => t.GwclRegionId, cascadeDelete: false)
                .Index(t => t.GwclRegionId)
                .Index(t => t.GwclStationId);
            
            CreateTable(
                "dbo.GwclStation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StationCode = c.String(nullable: false),
                        GwclRegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclRegion", t => t.GwclRegionId, cascadeDelete: false)
                .Index(t => t.GwclRegionId);
            
            CreateTable(
                "dbo.StationSystem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GwclStationId = c.Int(nullable: false),
                        WSystemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WSystem", t => t.WSystemID, cascadeDelete: false)
                .ForeignKey("dbo.GwclStation", t => t.GwclStationId, cascadeDelete: false)
                .Index(t => t.GwclStationId)
                .Index(t => t.WSystemID);
            
            CreateTable(
                "dbo.WSystem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 20),
                        Capacity = c.Int(nullable: false),
                        GwclStationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclStation", t => t.GwclStationId, cascadeDelete: false)
                .Index(t => t.GwclStationId);
            
            CreateTable(
                "dbo.SystemProduction",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WSystemId = c.Int(nullable: false),
                        ProductionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Production", t => t.ProductionId, cascadeDelete: false)
                .ForeignKey("dbo.WSystem", t => t.WSystemId, cascadeDelete: false)
                .Index(t => t.WSystemId)
                .Index(t => t.ProductionId);
            
            CreateTable(
                "dbo.Production",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DayToRecord = c.DateTime(nullable: false),
                        DailyActual = c.Double(nullable: false),
                        Comment = c.String(maxLength: 2000),
                        FRPH = c.Double(nullable: false),
                        FRPS = c.Double(nullable: false),
                        TFPD = c.Double(nullable: false),
                        NTFPD = c.Double(nullable: false),
                        LOG = c.Double(nullable: false),
                        WSystemId = c.Int(nullable: false),
                        OptionId = c.Int(nullable: false),
                        OptionTypeId = c.Int(nullable: false),
                        GwclStationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclStation", t => t.GwclStationId, cascadeDelete: false)
                .ForeignKey("dbo.Option", t => t.OptionId, cascadeDelete: false)
                .ForeignKey("dbo.OptionType", t => t.OptionTypeId, cascadeDelete: false)
                .ForeignKey("dbo.WSystem", t => t.WSystemId, cascadeDelete: false)
                .Index(t => t.WSystemId)
                .Index(t => t.OptionId)
                .Index(t => t.OptionTypeId)
                .Index(t => t.GwclStationId);
            
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
                "dbo.WSystemPlantDowntime",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WSystemId = c.Int(nullable: false),
                        PlantDowntimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PlantDowntime", t => t.PlantDowntimeId, cascadeDelete: false)
                .ForeignKey("dbo.WSystem", t => t.WSystemId, cascadeDelete: false)
                .Index(t => t.WSystemId)
                .Index(t => t.PlantDowntimeId);
            
            CreateTable(
                "dbo.PlantDowntime",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CurrentDate = c.DateTime(nullable: false),
                        Starttime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        HoursDown = c.Int(nullable: false),
                        WSystemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WSystem", t => t.WSystemId, cascadeDelete: false)
                .Index(t => t.WSystemId);
            
            CreateTable(
                "dbo.UserStation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GwclStationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclStation", t => t.GwclStationId, cascadeDelete: false)
                .Index(t => t.GwclStationId);
            
            CreateTable(
                "dbo.UserRegion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GwclRegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclRegion", t => t.GwclRegionId, cascadeDelete: false)
                .Index(t => t.GwclRegionId);
            
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
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.ConfigData",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
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
                        GwclRegionID = c.Int(nullable: false),
                        GwclStationId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GwclRegion", t => t.GwclRegionID, cascadeDelete: false)
                .ForeignKey("dbo.GwclStation", t => t.GwclStationId, cascadeDelete: false)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.GwclRegionID)
                .Index(t => t.GwclStationId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.User", "GwclStationId", "dbo.GwclStation");
            DropForeignKey("dbo.User", "GwclRegionID", "dbo.GwclRegion");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.AreaRegion", "GwclRegionId", "dbo.GwclRegion");
            DropForeignKey("dbo.UserRegion", "GwclRegionId", "dbo.GwclRegion");
            DropForeignKey("dbo.RegionStation", "GwclRegionId", "dbo.GwclRegion");
            DropForeignKey("dbo.RegionStation", "GwclStationId", "dbo.GwclStation");
            DropForeignKey("dbo.UserStation", "GwclStationId", "dbo.GwclStation");
            DropForeignKey("dbo.StationSystem", "GwclStationId", "dbo.GwclStation");
            DropForeignKey("dbo.StationSystem", "WSystemID", "dbo.WSystem");
            DropForeignKey("dbo.WSystemPlantDowntime", "WSystemId", "dbo.WSystem");
            DropForeignKey("dbo.WSystemPlantDowntime", "PlantDowntimeId", "dbo.PlantDowntime");
            DropForeignKey("dbo.PlantDowntime", "WSystemId", "dbo.WSystem");
            DropForeignKey("dbo.SystemProduction", "WSystemId", "dbo.WSystem");
            DropForeignKey("dbo.SystemProduction", "ProductionId", "dbo.Production");
            DropForeignKey("dbo.Production", "WSystemId", "dbo.WSystem");
            DropForeignKey("dbo.Production", "OptionTypeId", "dbo.OptionType");
            DropForeignKey("dbo.Production", "OptionId", "dbo.Option");
            DropForeignKey("dbo.Production", "GwclStationId", "dbo.GwclStation");
            DropForeignKey("dbo.WSystem", "GwclStationId", "dbo.GwclStation");
            DropForeignKey("dbo.GwclStation", "GwclRegionId", "dbo.GwclRegion");
            DropForeignKey("dbo.GwclRegion", "GwclAreaID", "dbo.GwclArea");
            DropForeignKey("dbo.AreaRegion", "GwclAreaId", "dbo.GwclArea");
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.User", new[] { "GwclStationId" });
            DropIndex("dbo.User", new[] { "GwclRegionID" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserRegion", new[] { "GwclRegionId" });
            DropIndex("dbo.UserStation", new[] { "GwclStationId" });
            DropIndex("dbo.PlantDowntime", new[] { "WSystemId" });
            DropIndex("dbo.WSystemPlantDowntime", new[] { "PlantDowntimeId" });
            DropIndex("dbo.WSystemPlantDowntime", new[] { "WSystemId" });
            DropIndex("dbo.Production", new[] { "GwclStationId" });
            DropIndex("dbo.Production", new[] { "OptionTypeId" });
            DropIndex("dbo.Production", new[] { "OptionId" });
            DropIndex("dbo.Production", new[] { "WSystemId" });
            DropIndex("dbo.SystemProduction", new[] { "ProductionId" });
            DropIndex("dbo.SystemProduction", new[] { "WSystemId" });
            DropIndex("dbo.WSystem", new[] { "GwclStationId" });
            DropIndex("dbo.StationSystem", new[] { "WSystemID" });
            DropIndex("dbo.StationSystem", new[] { "GwclStationId" });
            DropIndex("dbo.GwclStation", new[] { "GwclRegionId" });
            DropIndex("dbo.RegionStation", new[] { "GwclStationId" });
            DropIndex("dbo.RegionStation", new[] { "GwclRegionId" });
            DropIndex("dbo.GwclRegion", new[] { "GwclAreaID" });
            DropIndex("dbo.AreaRegion", new[] { "GwclRegionId" });
            DropIndex("dbo.AreaRegion", new[] { "GwclAreaId" });
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.SMS_IN");
            DropTable("dbo.ConfigData");
            DropTable("dbo.Role");
            DropTable("dbo.Error");
            DropTable("dbo.UserRegion");
            DropTable("dbo.UserStation");
            DropTable("dbo.PlantDowntime");
            DropTable("dbo.WSystemPlantDowntime");
            DropTable("dbo.OptionType");
            DropTable("dbo.Option");
            DropTable("dbo.Production");
            DropTable("dbo.SystemProduction");
            DropTable("dbo.WSystem");
            DropTable("dbo.StationSystem");
            DropTable("dbo.GwclStation");
            DropTable("dbo.RegionStation");
            DropTable("dbo.GwclArea");
            DropTable("dbo.GwclRegion");
            DropTable("dbo.AreaRegion");
        }
    }
}
