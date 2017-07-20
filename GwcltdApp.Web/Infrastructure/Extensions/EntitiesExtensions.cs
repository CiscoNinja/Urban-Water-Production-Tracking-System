using GwcltdApp.Entities;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Extensions
{
    public static class EntitiesExtensions
    {
        public static void UpdateProduction(this Production production, ProductionViewModel productionVm)
        {
            production.Comment = productionVm.Comment;
            production.DailyActual = productionVm.DailyActual;
            production.DateCreated = productionVm.DateCreated;
            production.DayToRecord = productionVm.DayToRecord;
            production.FRPH = productionVm.FRPH;
            production.FRPS = productionVm.FRPS;
            production.TFPD = productionVm.TFPD;
            production.NTFPD = productionVm.NTFPD;
            production.LOG = productionVm.LOG;
            production.OptionId = productionVm.OptionId;
            production.OptionTypeId = productionVm.OptionTypeId;
            production.WSystemId = productionVm.WSystemId;
            production.GwclStationId = productionVm.GwclStationId;
        }

        public static void UpdateHrlyProduction(this HourlyProduction production, ProductionViewModel productionVm)
        {
            production.Comment = productionVm.Comment;
            production.HourlyActual = productionVm.TFPD;
            production.DateCreated = productionVm.DateCreated;
            production.DayToRecord = productionVm.DayToRecord;
            production.FRPH = productionVm.FRPH;
            production.FRPS = productionVm.FRPS;
            production.TFPD = productionVm.TFPD;
            production.NTFPD = productionVm.NTFPD;
            production.LOG = productionVm.LOG;
            production.OptionId = productionVm.OptionId;
            production.OptionTypeId = productionVm.OptionTypeId;
            production.WSystemId = productionVm.WSystemId;
            production.GwclStationId = productionVm.GwclStationId;
        }

        public static void AddProduction(this Production production, ProductionViewModel productionVm)
        {
            using (Data.GwcltdAppContext context = new Data.GwcltdAppContext())
            {
                production.Comment = productionVm.Comment;
                production.DailyActual = productionVm.DailyActual;
                production.DateCreated = productionVm.DateCreated;
                production.DayToRecord = productionVm.DayToRecord;
                production.FRPH = productionVm.FRPH;
                production.FRPS = productionVm.FRPS;
                production.TFPD = productionVm.TFPD;
                production.NTFPD = productionVm.NTFPD;
                production.LOG = productionVm.LOG;
                production.OptionId = productionVm.OptionId;
                production.OptionTypeId = productionVm.OptionTypeId;
                production.WSystemId = productionVm.WSystemId;
                production.GwclStationId = productionVm.GwclStationId;

                context.ProductionSet.Add(production);

                context.SaveChanges();
            }
        }

        public static void AddHrlyProduction(this HourlyProduction hrlyproduction, ProductionViewModel productionVm)
        {
            using (Data.GwcltdAppContext context = new Data.GwcltdAppContext())
            {
                hrlyproduction.Comment = productionVm.Comment;
                hrlyproduction.HourlyActual = productionVm.DailyActual;
                hrlyproduction.DateCreated = productionVm.DateCreated;
                hrlyproduction.DayToRecord = productionVm.DayToRecord;
                hrlyproduction.FRPH = productionVm.FRPH;
                hrlyproduction.FRPS = productionVm.FRPS;
                hrlyproduction.TFPD = productionVm.TFPD;
                hrlyproduction.NTFPD = productionVm.NTFPD;
                hrlyproduction.LOG = productionVm.LOG;
                hrlyproduction.OptionId = productionVm.OptionId;
                hrlyproduction.OptionTypeId = productionVm.OptionTypeId;
                hrlyproduction.WSystemId = productionVm.WSystemId;
                hrlyproduction.GwclStationId = productionVm.GwclStationId;

                context.HourlyProductionSet.Add(hrlyproduction);

                context.SaveChanges();
            }
        }

        public static void UpdatePlantDowntime(this PlantDowntime plantdowntime, PlantDowntimeViewModel plantdowntimeVm)
        {
            plantdowntime.CurrentDate = plantdowntimeVm.CurrentDate;
            plantdowntime.EndTime = plantdowntimeVm.EndTime;
            plantdowntime.HoursDown = plantdowntimeVm.HoursDown ;
            plantdowntime.Starttime = plantdowntimeVm.Starttime;
            plantdowntime.WSystemId = plantdowntimeVm.WSystemId;
        }

        public static void UpdateWArea(this GwclArea gwclarea, GwclAreaViewModel gwclareaVm)
        {
            gwclarea.Name = gwclareaVm.Name;
            gwclarea.Code = gwclareaVm.Code;
        }

        public static void UpdateRegion(this GwclRegion gwclregion, GwclRegionViewModel gwclregionVm)
        {
            gwclregion.Name = gwclregionVm.Name;
            gwclregion.Code = gwclregionVm.Code;
            gwclregion.GwclAreaID = gwclregionVm.GwclAreaID;
        }

        public static void UpdateStation(this GwclStation gwclstation, GwclStationViewModel gwclstationVm)
        {
            gwclstation.Name = gwclstationVm.Name;
            gwclstation.StationCode = gwclstationVm.StationCode;
            gwclstation.GwclRegionId = gwclstationVm.GwclRegionId;
        }

        public static void UpdateSystem(this WSystem gwclsysytem, WSystemViewModel gwclsysytemVm)
        {
            gwclsysytem.Name = gwclsysytemVm.Name;
            gwclsysytem.Code = gwclsysytemVm.Code;
            gwclsysytem.Capacity = gwclsysytemVm.Capacity;
            gwclsysytem.GwclStationId = gwclsysytemVm.GwclStationId;
        }

        public static void UpdateOption(this Option gwcloption, OptionViewModel gwcloptionVm)
        {
            gwcloption.Name = gwcloptionVm.Name;
            gwcloption.OptionOf = gwcloptionVm.OptionOf;
        }

        public static void UpdateType(this OptionType gwclotypes, OptionTypeViewModel gwclotypesVm)
        {
            gwclotypes.Name = gwclotypesVm.Name;
        }
    }
}