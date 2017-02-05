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
            gwclsysytem.Name = gwclsysytem.Name;
            gwclsysytem.Code = gwclsysytem.Code;
            gwclsysytem.Capacity = gwclsysytem.Capacity;
            gwclsysytem.GwclStationId = gwclsysytem.GwclStationId;
        }
    }
}