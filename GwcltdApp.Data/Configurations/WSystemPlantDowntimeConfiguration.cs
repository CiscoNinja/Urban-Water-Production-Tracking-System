using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class WSystemPlantDowntimeConfiguration : EntityBaseConfiguration<WSystemPlantDowntime>
    {
        public WSystemPlantDowntimeConfiguration()
        {
            Property(wpd => wpd.PlantDowntimeId).IsRequired();
            Property(wpd => wpd.WSystemId).IsRequired();
        }
    }
}
