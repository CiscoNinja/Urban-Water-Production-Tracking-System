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
        public static List<Production> GetAllProductions()
        {
            using (GwcltdAppContext context = new GwcltdAppContext())
            {
                return context.ProductionSet.OrderBy(x => x.DayToRecord).ToList();
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