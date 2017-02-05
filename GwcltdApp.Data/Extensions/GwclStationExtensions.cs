using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclStationExtensions
    {
        public static bool StationExists(this IEntityBaseRepository<GwclStation> gwclstationsRepository, string name, string code)
        {
            bool _stationExists = false;

            _stationExists = gwclstationsRepository.GetAll()
                .Any(c => c.Name.ToLower() == name ||
                c.StationCode.ToLower() == code);

            return _stationExists;
        }
    }
}
