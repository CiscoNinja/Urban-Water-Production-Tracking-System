using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class StationSystemConfiguration : EntityBaseConfiguration<StationSystem>
    {
        public StationSystemConfiguration()
        {
            Property(ss => ss.GwclStationId).IsRequired();
            Property(ss => ss.WSystemID).IsRequired();
        }
    }
}
