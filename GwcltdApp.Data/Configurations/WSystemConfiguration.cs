using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class WSystemConfiguration : EntityBaseConfiguration<WSystem>
    {
        public WSystemConfiguration()
        {
            Property(ws => ws.Code).IsRequired();
            Property(ws => ws.Name).IsRequired();
            Property(ws => ws.Capacity).IsRequired();
        }
    }
}
