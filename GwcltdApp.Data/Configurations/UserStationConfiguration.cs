using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data.Configurations
{
    public class UserStationConfiguration : EntityBaseConfiguration<UserStation>
    {
        public UserStationConfiguration()
        {
            Property(ur => ur.UserId).IsRequired();
            Property(ur => ur.GwclStationId).IsRequired();
        }
    }
}
