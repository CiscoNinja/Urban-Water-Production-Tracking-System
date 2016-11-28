using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class TypeOptionConfiguration : EntityBaseConfiguration<OptionType>
    {
        public TypeOptionConfiguration()
        {
            Property(g => g.Name).IsRequired().HasMaxLength(50);
        }
    }
}
