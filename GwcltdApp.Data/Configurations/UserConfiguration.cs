using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class UserConfiguration : EntityBaseConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.Username).IsRequired();
            Property(u => u.Email).IsRequired();
            Property(u => u.HashedPassword).IsRequired();
            Property(u => u.Salt).IsRequired();
            Property(u => u.IsLocked).IsRequired();
            Property(u => u.DateCreated);
            Property(u => u.GwclRegionID).IsRequired();
            Property(u => u.GwclStationId).IsRequired();
            Property(u => u.RoleId).IsRequired();
        }
    }
}
