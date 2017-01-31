using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class UserRegionConfiguration : EntityBaseConfiguration<UserRegion>
    {
        public UserRegionConfiguration()
        {
            Property(ur => ur.UserId).IsRequired();
            Property(ur => ur.GwclRegionId).IsRequired();
        }
    }
}
