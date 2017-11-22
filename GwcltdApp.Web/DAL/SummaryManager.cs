using GwcltdApp.Data;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.DAL
{
    public class SummaryManager
    {
        public static List<WSystem> GetAllSystems()//change code to get systems from the logged in user's station
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.OrderBy(x => x.GwclStationId).ToList();
            }
        }
        public static List<ExcelCell> GetCells()//change code to get systems from the logged in user's station
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ExcelCellSet.ToList();
            }
        }
        public static int GetOptionIdByName(string oname)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.OptionSet.Where(s => s.Name == oname).Select(s => s.ID).Single();
            }
        }
        public static int GetOptionTypeIdByName(string otname)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.OptionTypeSet.Where(s => s.Name == otname).Select(s => s.ID).Single();
            }
        }
        public static List<WSystem> GetAllUserSystems(int userstation)//change code to get systems from the logged in user's station
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(s => s.GwclStationId == userstation).OrderBy(x => x.Code).ToList();
            }
        }

        public static string GetSystemName(string code)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(x => x.Code == code).Select(x => x.Name).Single();
            }
        }
        public static string GetSystemCode(int id)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(x => x.ID == id).Select(x => x.Code).Single();
            }
        }

        public static int GetSystemId(string code)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(x => x.Code == code).Select(x => x.ID).Single();
            }
        }

        public static string GetStationName(string code)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(x => x.Code == code).FirstOrDefault().GwclStation.Name;
            }
        }

        public static int GetStationId(string code)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(x => x.Code == code).FirstOrDefault().GwclStationId;
            }
        }

        public static string GetStationCode(string code)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.Where(x => x.Code == code).FirstOrDefault().GwclStation.StationCode;
            }
        }

        public static double GetTFbyDate(DateTime date, int systemId, int wtype) //gets total flow by date and water type (eg. Raw Water)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet.Where(x => (DbFunctions.TruncateTime(x.DayToRecord) == date.Date) && x.WSystemId == systemId && x.OptionId == wtype).Select(x => x.TFPD).DefaultIfEmpty().Single();
            }
        }


        public static int GetTypeIdByName(string name)// gets id of water options Treated or Raw
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.OptionSet.Where(x => x.Name == name).Select(x => x.ID).Single();
            }
        }

        public static int GetTypeIdByName2(string name)// get type of water option Treated water 1 etx
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.OptionTypeSet.Where(x => x.Name == name).Select(x => x.ID).Single();
            }
        }
        public static double DailyActual(double currentTotal, DateTime previousDate, int sysId, int waterType)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                var previoousTotal = GetTFbyDate(previousDate, sysId, waterType);
                double res = currentTotal - previoousTotal;
                return Math.Round(res, 2, MidpointRounding.AwayFromZero);
            }
        }

        public static double PlantLoss_percent(string itemcode, int yr, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double raw = 0;
                switch (yr)
                {
                    case 1000:
                        {
                            raw = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month
                            == mnth && x.Option.Name.Equals("Raw Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                            break;
                        }
                    default:
                        {
                            raw = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Year
                            == yr && x.DayToRecord.Month == mnth && x.Option.Name.Equals("Raw Water"))
                            .Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                            break;
                        }
                }
                double res = 0;
                if (raw != 0)
                {
                    res = (PlantLoss_metre(itemcode, yr, mnth) / (double)raw) * 100;
                }
                return Math.Round(res, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double PlantLoss_metre(string itemcode, int yr, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double ploss = 0;
                switch (yr)
                {
                    case 1000:
                        {
                            ploss = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month == mnth 
                            && x.Option.Name.Equals("Raw Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum()
                            - context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month == mnth
                            && x.Option.Name.Equals("Treated Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                            break;
                        }
                    default:
                        {
                            ploss = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Year == yr 
                            && x.DayToRecord.Month == mnth && x.Option.Name.Equals("Raw Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum()
                            - context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Year == yr && x.DayToRecord.Month == mnth
                            && x.Option.Name.Equals("Treated Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                            break;
                        }
                }
                return Math.Round(ploss, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double getWaterTable(string itemcode, int mnth, string watertype)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double wlevel = context.ProductionSet
                    .Where(x => x.WSystem.Code
                    == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals(watertype)).Select(x => x.DailyActual).DefaultIfEmpty().Sum();

                return Math.Round(wlevel, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double getWaterTableSingle(string itemcode, int yr, int mnth, string watertype)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double wlevel = 0;
                switch (yr)
                {
                    case 1000:
                        {
                            wlevel = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month == mnth 
                            && x.Option.Name.Equals(watertype)).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                            break;
                        }
                    default:
                        {
                            wlevel = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Year == yr
                            && x.DayToRecord.Month == mnth && x.Option.Name.Equals(watertype)).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                            break;
                        }
                }
                return Math.Round(wlevel, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double getPlantCap(string itemcode, int yr, int mnth, string systm)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                int syscapacity = context.WSystemSet.Where(x => x.Code == systm).FirstOrDefault().Capacity;
                double res = 0;
                if (syscapacity != 0)
                {
                    switch (yr)
                    {
                        case 1000:
                            {
                                res = (getDailyAverage(itemcode, mnth) / (double)syscapacity) * 100;
                                break;
                            }
                        default:
                            {
                                res = (getDailyAverageSingle(itemcode, yr, mnth) / (double)syscapacity) * 100;
                                break;
                            }
                    }
                }
                return Math.Round(res, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double getDailyAverage(string itemcode, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double returnval = 0;
                //checkes if there is data for a system
                bool systemHasValue = getWaterTable(itemcode, mnth, "Treated Water") > 0;

                if (systemHasValue)
                {
                    DateTime date = context.ProductionSet
                   .Where(x => x.WSystem.Code
                   == itemcode && x.DayToRecord.Month
                   == mnth && x.Option.Name.Equals("Treated Water")).FirstOrDefault().DayToRecord;

                    var totalformonth = getWaterTable(itemcode, mnth, "Treated Water");

                    if (totalformonth != 0)
                    {
                        returnval = totalformonth / (double)DateTime.DaysInMonth(date.Year, mnth);
                    }
                }

                return Math.Round(returnval, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double getDailyAverageSingle(string itemcode, int yr, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double returnval = 0;
                switch (yr)
                {
                    case 1000:
                        {
                            //checkes if there is data for a system
                            bool systemHasValue = getWaterTable(itemcode, mnth, "Treated Water") > 0;

                            if (systemHasValue)
                            {
                                DateTime date = context.ProductionSet
                               .Where(x => x.WSystem.Code
                               == itemcode && x.DayToRecord.Month
                               == mnth && x.Option.Name.Equals("Treated Water")).FirstOrDefault().DayToRecord;

                                var totalformonth = getWaterTable(itemcode, mnth, "Treated Water");

                                if (totalformonth != 0)
                                {
                                    returnval = totalformonth / (double)DateTime.DaysInMonth(date.Year, mnth);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            //checkes if there is data for a system
                            bool systemHasValue = getWaterTableSingle(itemcode, yr, mnth, "Treated Water") > 0;

                            if (systemHasValue)
                            {
                                DateTime date = context.ProductionSet
                               .Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Year == yr && x.DayToRecord.Month == mnth 
                               && x.Option.Name.Equals("Treated Water")).FirstOrDefault().DayToRecord;

                                var totalformonth = getWaterTableSingle(itemcode, yr, mnth, "Treated Water");

                                if (totalformonth != 0)
                                {
                                    returnval = totalformonth / (double)DateTime.DaysInMonth(date.Year, mnth);
                                }
                            }
                            break;
                        }
                }
                
                return Math.Round(returnval, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static List<DateTime> GetAllDates()
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet.Select(x => x.DayToRecord).ToList();
            }
        }
    }
}