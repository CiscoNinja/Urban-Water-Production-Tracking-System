using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class ProductionConfiguration : EntityBaseConfiguration<Production>
    {
        public ProductionConfiguration()
        {
            Property(pc => pc.DateCreated).IsRequired();
            Property(pc => pc.DayToRecord).IsRequired();
            Property(pc => pc.DailyActual).IsRequired();
            Property(pc => pc.FRPH).IsRequired();
            Property(pc => pc.FRPS).IsRequired();
            Property(pc => pc.TFPD).IsRequired();
            Property(pc => pc.NTFPD).IsRequired();
            Property(pc => pc.LOG).IsRequired();
            Property(pc => pc.Comment).IsOptional().HasMaxLength(2000);
            Property(o => o.OptionId).IsRequired();
            Property(ot => ot.OptionTypeId).IsRequired();
            Property(ws => ws.WSystemId).IsRequired();
            Property(ws => ws.GwclStationId).IsRequired();
        }
    }
}
