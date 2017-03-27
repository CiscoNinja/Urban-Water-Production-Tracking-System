using GwcltdApp.Data;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
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
                return context.WSystemSet.OrderBy(x => x.Code).ToList();
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

        public static double GetTFbyDate(DateTime date) //gets total flow by date
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet.Where(x => x.DayToRecord == date).Select(x => x.TFPD).DefaultIfEmpty().Single();
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
        public static double DailyActual(DateTime currentDate, DateTime previousDate)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {

                var currentTotal = GetTFbyDate(currentDate);
                var previoousTotal = GetTFbyDate(previousDate);

                double res = currentTotal;
                if (previoousTotal != 0)
                {
                    res = currentTotal - previoousTotal;
                }
                return Math.Round(res, 2, MidpointRounding.AwayFromZero);
            }
        }

        public static double PlantLoss_percent(string itemcode, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
               
                var raw = context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals("Raw Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                
                double res = 0;
                if (raw != 0)
                {
                    res = (PlantLoss_metre(itemcode, mnth) / (double)raw)*100;
                }
                return Math.Round(res, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double PlantLoss_metre(string itemcode, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                double ploss = context.ProductionSet
                    .Where(x => x.WSystem.Code
                    == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals("Raw Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum()
                    - context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month == mnth && x.Option.Name.Equals("Treated Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum();

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
        public static double getPlantCap(string itemcode, int mnth, string systm)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                int syscapacity = context.WSystemSet.Where(x => x.Code == systm).FirstOrDefault().Capacity;
                double res = 0;
                if (syscapacity != 0)
                {
                    res = (getDailyAverage(itemcode, mnth) / (double)syscapacity) * 100;
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
        public static List<DateTime> GetAllDates()
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet.Select(x => x.DayToRecord).ToList();
            }
        }
    }
}