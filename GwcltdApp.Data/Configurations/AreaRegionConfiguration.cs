using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class AreaRegionConfiguration : EntityBaseConfiguration<AreaRegion>
    {
        public AreaRegionConfiguration()
        {
            Property(ar => ar.GwclAreaId).IsRequired();
            Property(ar => ar.GwclRegionId).IsRequired();
        }
    }
}
