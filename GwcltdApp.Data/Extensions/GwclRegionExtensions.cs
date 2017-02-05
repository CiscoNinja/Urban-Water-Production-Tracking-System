using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Extensions
{
    public static class GwclRegionExtensions
    {
        public static bool RegionExists(this IEntityBaseRepository<GwclRegion> gwclregionsRepository, string name, string code)
        {
            bool _regionExists = false;

            _regionExists = gwclregionsRepository.GetAll()
                .Any(c => c.Name.ToLower() == name ||
                c.Code.ToLower() == code);

            return _regionExists;
        }
    }
}
