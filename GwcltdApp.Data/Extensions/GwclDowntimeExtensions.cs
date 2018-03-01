using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclDowntimeExtensions
    {
        public static bool DowntimeExists(this IEntityBaseRepository<PlantDowntime> gwcldowntimeRepository, DateTime currentDate, DateTime startTime,
            DateTime endTime, int systemId, int hoursDown)
        {
            bool _pdowntimeExists = false;

            _pdowntimeExists = gwcldowntimeRepository.GetAll()
                .Any(c => c.CurrentDate == currentDate && c.Starttime == startTime
                && c.EndTime == endTime && c.WSystemId == systemId && c.HoursDown == hoursDown);

            return _pdowntimeExists;
        }
    }
}
