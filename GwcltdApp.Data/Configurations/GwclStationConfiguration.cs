using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class GwclStationConfiguration : EntityBaseConfiguration<GwclStation>
    {
        public GwclStationConfiguration()
        {
            Property(gs => gs.StationCode).IsRequired();
            Property(gs => gs.Name).IsRequired();
        }
    }
}
