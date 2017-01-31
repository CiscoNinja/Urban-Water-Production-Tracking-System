using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class GwclArea : IEntityBase
    {
        public GwclArea()
        {
            AreaRegions = new List<AreaRegion>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<AreaRegion> AreaRegions { get; set; }
    }
}
