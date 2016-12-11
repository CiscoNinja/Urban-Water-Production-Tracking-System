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
        public static List<WSystem> GetAllSystems()
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.WSystemSet.OrderBy(x => x.Code).ToList();
            }
        }
        public static string GetTypeName(int id)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.OptionSet.Where(x => x.ID == id).Select(x => x.ID).ToString();
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
                return res;
            }
        }
        public static int PlantLoss_metre(string itemcode, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet
                    .Where(x => x.WSystem.Code
                    == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals("Raw Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum()
                    - context.ProductionSet.Where(x => x.WSystem.Code == itemcode && x.DayToRecord.Month == mnth && x.Option.Name.Equals("Treated Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
            }
        }
        public static int getWaterTable(string itemcode, int mnth, string watertype)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet
                    .Where(x => x.WSystem.Code
                    == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals(watertype)).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
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

                return res;
            }
        }
        public static double getDailyAverage(string itemcode, int mnth)
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                DateTime date = context.ProductionSet
                    .Where(x => x.WSystem.Code
                    == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals("Treated Water")).FirstOrDefault().DayToRecord;

                return context.ProductionSet
                    .Where(x => x.WSystem.Code
                    == itemcode && x.DayToRecord.Month
                    == mnth && x.Option.Name.Equals("Treated Water")).Select(x => x.DailyActual).DefaultIfEmpty().Sum() /  (double)DateTime.DaysInMonth(date.Year, mnth);
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