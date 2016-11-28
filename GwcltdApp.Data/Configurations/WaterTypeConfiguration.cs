using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class WaterTypeConfiguration : EntityBaseConfiguration<Option>
    {
        public WaterTypeConfiguration()
        {
            Property(ot => ot.Name).IsRequired().HasMaxLength(50);
        }
    }
}
