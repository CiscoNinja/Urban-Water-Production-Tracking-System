using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class GwclAreaConfiguration : EntityBaseConfiguration<GwclArea>
    {
        public GwclAreaConfiguration()
        {
            Property(gc => gc.Code).IsRequired();
            Property(gc => gc.Name).IsRequired();
        }
    }
}
