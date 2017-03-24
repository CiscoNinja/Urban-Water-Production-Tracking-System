using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class UserRegion : IEntityBase
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int GwclRegionId { get; set; }
        public virtual User User { get; set; }
    }
}
