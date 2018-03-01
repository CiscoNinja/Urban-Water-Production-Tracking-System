using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclProductionExtensions
    {
        public static bool ProductionExists(this IEntityBaseRepository<HourlyProduction> gwclproductionRepository, DateTime dayToRecord, int systemId,
            int optionId, int optiontypeId, int stationId, double hourlyActual)
        {
            bool _productionExists = false;

            _productionExists = gwclproductionRepository.GetAll()
                .Any(c => c.DayToRecord == dayToRecord && c.HourlyActual == hourlyActual 
                && c.OptionId == optionId && c.WSystemId == systemId && c.OptionTypeId == optiontypeId
                && c.GwclStationId == stationId);

            return _productionExists;
        }
    }
}
