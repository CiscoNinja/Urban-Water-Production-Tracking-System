using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class SystemProductionConfiguration : EntityBaseConfiguration<SystemProduction>
    {
        public SystemProductionConfiguration()
        {
            Property(sp => sp.ProductionId).IsRequired();
            Property(sp => sp.WSystemId).IsRequired();
        }
    }
}
