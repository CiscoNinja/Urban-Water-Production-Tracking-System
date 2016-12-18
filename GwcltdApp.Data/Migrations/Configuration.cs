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
            //create options
            //create options
            context.OptionSet.AddOrUpdate(o => o.Name, GenerateOptions());
            //create optiontypes
            context.OptionTypeSet.AddOrUpdate(ot => ot.Name, GenerateOptionTypes());
            //create systems
            context.WSystemSet.AddOrUpdate(ws => ws.Name, GenerateWSystems());
            //create roles
            context.RoleSet.AddOrUpdate(r => r.Name, GenerateRoles());
            //create productions
            context.ProductionSet.AddOrUpdate(GenerateProductions());

            // username: chsakell, password: homecinema
            context.UserSet.AddOrUpdate(u => u.Email, new User[]{
                new User()
                {
                    Email="chsakells.blog@gmail.com",
                    Username="chsakell",
                    HashedPassword ="XwAQoiq84p1RUzhAyPfaMDKVgSwnn80NCtsE8dNv3XI=",
                    Salt = "mNKLRbEFCH8y1xIyTXP4qA==",
                    IsLocked = false,
                    DateCreated = DateTime.Now
                }
            });

            // // create user-admin for chsakell
            context.UserRoleSet.AddOrUpdate(new UserRole[] {
                new UserRole() {
                    RoleId = 1, // admin
                    UserId = 1  // chsakell
                }
            });
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
                new WSystem() { Name = "Kpong New", Code = "S01", Capacity = 181818},
                new WSystem() { Name = "Kpong Old", Code = "S02", Capacity = 38636 },
                new WSystem() { Name = "Keseve/Adafoa", Code = "S03", Capacity = 1363 },
                new WSystem() { Name = "Tahal", Code = "S04", Capacity = 100},
                new WSystem() { Name = "China Ghezhouba", Code = "S05", Capacity = 838 },
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
                    int optionid = r.Next(7, 9);
                    int trtoptions = r.Next(9, 13); //takes optiontype from treated water option
                    int rawoption = r.Next(13, 17); //takes optiontype from raw water option
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
                        OptionTypeId = (optionid == 7) ? trtoptions : rawoption,

                    };
                    _productions.Add(production);
                }
            }
            return _productions.ToArray();
        }
    }
}

