using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    class PlantDowntimeConfiguration : EntityBaseConfiguration<PlantDowntime>
    {
        public PlantDowntimeConfiguration()
        {
            Property(pd => pd.CurrentDate).IsRequired();
            Property(pd => pd.EndTime).IsRequired();
            Property(pd => pd.HoursDown).IsRequired();
            Property(pd => pd.Starttime).IsRequired();
            Property(pd => pd.WSystemId).IsRequired();
        }
    }
}
