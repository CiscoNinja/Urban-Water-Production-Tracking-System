namespace GwcltdApp.Data.Migrations
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GwcltdAppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GwcltdAppContext context)
        {
            //context.OptionSet.AddOrUpdate(o => o.Name, GenerateOptions());
            //context.OptionTypeSet.AddOrUpdate(ot => ot.Name, GenerateOptionTypes());
            //context.GwclAreaSet.AddOrUpdate(GenerateGwclAreas());
            //context.GwclRegionSet.AddOrUpdate(GenerateGwclRegions());
            //context.GwclStationSet.AddOrUpdate(GenerateGwclStations());
            //context.WSystemSet.AddOrUpdate(ws => ws.Name, GenerateWSystems());
            //////create roles
            //context.RoleSet.AddOrUpdate(r => r.Name, GenerateRoles());
            //create productions
            //context.ProductionSet.AddOrUpdate(GenerateProductions());

            // username: chsakell, password: homecinema
            //context.UserSet.AddOrUpdate(u => u.Email, new User[]{
            //    new User()
            //    {
            //        Email="chsakells.blog@gmail.com",
            //        Username="chsakell",
            //        HashedPassword ="XwAQoiq84p1RUzhAyPfaMDKVgSwnn80NCtsE8dNv3XI=",
            //        Salt = "mNKLRbEFCH8y1xIyTXP4qA==",
            //        IsLocked = false,
            //        DateCreated = DateTime.Now,
            //        GwclRegionID = 1
            //    }
            //});

            //username: ciscomaria5, password: 3GoDsinone
            //context.UserSet.AddOrUpdate(u => u.Email, new User[]{
            //    new User()
            //    {
            //        Email="franciscokadzi@gmail.com",
            //        Username="ciscomaria5",
            //        HashedPassword ="QmedqmUmT7/IVhKm0J7sTcDAsiUkQSVmdIz0vMlA+H4=",
            //        Salt = "aXdlHqpYS25mAYYSzn7Z+w==",
            //        IsLocked = false,
            //        DateCreated = DateTime.Now,
            //        GwclRegionID = 1,
            //        GwclStationId = 1,
            //        RoleId = 1
            //    }
            //});

            //context.UserRoleSet.AddOrUpdate(new UserRole[] {
            //     new UserRole() {
            //         RoleId = 1, // admin
            //         UserId = 1  // chsakell
            //     }
            //});

            //context.UserRegionSet.AddOrUpdate(new UserRegion[] {
            //     new UserRegion() {
            //         GwclRegionId = 1,
            //         UserId = 1
            //     }
            // });

            //context.UserStationSet.AddOrUpdate(new UserStation[] {
            //     new UserStation() {
            //         GwclStationId = 1,
            //         UserId = 1
            //     }
            // });


            //context.SystemProductionSet.AddOrUpdate(new SystemProduction[] {
            //    new SystemProduction() {
            //        ProductionId = 1,
            //        WSystemId = 1
            //    }
            //});

            //context.StationSystemSet.AddOrUpdate(new StationSystem[] {
            //    new StationSystem() {
            //        GwclStationId = 1,
            //        WSystemID = 1
            //    }
            //});

            //context.RegionStationSet.AddOrUpdate(new RegionStation[] {
            //    new RegionStation() {
            //        GwclRegionId = 1,
            //        GwclStationId = 1
            //    }
            //});

            //context.AreaRegionSet.AddOrUpdate(new AreaRegion[] {
            //    new AreaRegion() {
            //        GwclRegionId = 1,
            //        GwclAreaId = 1
            //    }
            //});

        }
        private Option[] GenerateOptions()
        {
            Option[] options = new Option[] {
                new Option() { Name = "Treated Water", OptionOf = "Productions"},
                new Option() { Name = "Raw Water", OptionOf = "Productions" },
                new Option() { Name = "Power", OptionOf = "PlantDowntimes" },
                new Option() { Name = "Unplanned Shutdown", OptionOf = "PlantDowntimes"  },
                new Option() { Name = "Planned Shutdown", OptionOf = "PlantDowntimes"  },
                new Option() { Name = "Others", OptionOf = "PlantDowntimes"  },
            };

            return options;
        }

        private GwclArea[] GenerateGwclAreas()
        {
            GwclArea[] gwclareas = new GwclArea[] {
                new GwclArea() { Name = "Kumasi", Code = "A01"}
            };

            return gwclareas;
        }
        private GwclRegion[] GenerateGwclRegions()
        {
            GwclRegion[] gwclregions = new GwclRegion[] {
                new GwclRegion() { Name = "RegKumasi", Code = "PR01"}
            };

            return gwclregions;
        }

        private GwclStation[] GenerateGwclStations()
        {
            GwclStation[] gwclstations = new GwclStation[] {
                new GwclStation() { Name = "Kpong", StationCode = "AA01"}
            };

            return gwclstations;
        }

        private OptionType[] GenerateOptionTypes()
        {
            OptionType[] optiontypes = new OptionType[] {
                new OptionType() { Name = "Treated Water 1"/*, OptionId = 1*/},
                new OptionType() { Name = "Treated Water 2"/*, OptionId = 1*/},
                new OptionType() { Name = "Treated Water 3"/*, OptionId = 1*/},
                new OptionType() { Name = "Treated Water 4"/*, OptionId = 1*/},
                new OptionType() { Name = "Raw Water 1"/*, OptionId = 2 */},
                new OptionType() { Name = "Raw Water 2"/*, OptionId = 2 */},
                new OptionType() { Name = "Raw Water 3"/*, OptionId = 2 */},
                new OptionType() { Name = "Raw Water 4"/*, OptionId = 2 */},
            };

            return optiontypes;
        }

        private WSystem[] GenerateWSystems()
        {
            WSystem[] wsystem = new WSystem[] {
                new WSystem() { Name = "Kpong New", Code = "S01", Capacity = 181818, GwclStationId =1 },
                new WSystem() { Name = "Kpong Old", Code = "S02", Capacity = 38636, GwclStationId =1 },
                new WSystem() { Name = "Keseve/Adafoa", Code = "S03", Capacity = 1363, GwclStationId =1 },
                new WSystem() { Name = "Tahal", Code = "S04", Capacity = 100, GwclStationId =1},
                new WSystem() { Name = "China Ghezhouba", Code = "S05", Capacity = 838, GwclStationId =1 },
                new WSystem() { Name = "Siemens", Code = "S06", Capacity = 0 },
                new WSystem() { Name = "Desalination", Code = "S07", Capacity = 0 },
            };

            return wsystem;
        }

        private Role[] GenerateRoles()
        {
            Role[] _roles = new Role[]{
                new Role()
                {
                    Name="Super"
                },
                new Role()
                {
                    Name="Admin"
                }
            };

            return _roles;
        }

        private Production[] GenerateProductions()
        {
            List<Production> _productions = new List<Production>();
            Random r = new Random();
            for (int i = 1; i <= 12; i++)
            {
                int numOfDays = DateTime.DaysInMonth(DateTime.Now.Year, i);
                for (int j = 1; j <= numOfDays; j++)
                {

                    int wsystemId = r.Next(1, 8);
                    int optionid = r.Next(1, 3);
                    int trtoptions = r.Next(1, 5); //takes optiontype from treated water option
                    int rawoption = r.Next(5, 9); //takes optiontype from raw water option
                    Production production = new Production()
                    {
                        DateCreated = new DateTime(2016, i, j),
                        DayToRecord = new DateTime(2016, i, j),
                        DailyActual = 176400 + i + j + r.Next(1000, 7000),
                        FRPH = r.Next(100, 500),
                        FRPS = r.Next(-1, 1),
                        TFPD = r.Next(60500, 100000),
                        NTFPD = r.Next(-100, -1),
                        LOG = r.Next(1, 100),
                        Comment = "Put your comments here.",
                        WSystemId = wsystemId,
                        OptionId = optionid,
                        OptionTypeId = (optionid == 1) ? trtoptions : rawoption,
                        GwclStationId = 1
                    };
                    _productions.Add(production);
                }
            }
            return _productions.ToArray();
        }
    }
}

