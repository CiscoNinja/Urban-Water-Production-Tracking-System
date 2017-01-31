using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class RegionStationConfiguration : EntityBaseConfiguration<RegionStation>
    {
        public RegionStationConfiguration()
        {
            Property(ar => ar.GwclStationId).IsRequired();
            Property(ar => ar.GwclRegionId).IsRequired();
        }
    }
}
