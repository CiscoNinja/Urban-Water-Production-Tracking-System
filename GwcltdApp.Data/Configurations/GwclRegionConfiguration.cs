using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class GwclRegionConfiguration : EntityBaseConfiguration<GwclRegion>
    {
        public GwclRegionConfiguration()
        {
            Property(gr => gr.Code).IsRequired();
            Property(gr => gr.Name).IsRequired();
            Property(gr => gr.GwclAreaID).IsRequired();
        }
    }
}
