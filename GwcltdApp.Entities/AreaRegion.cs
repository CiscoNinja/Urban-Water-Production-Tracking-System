using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class AreaRegion : IEntityBase
    {
        public int ID { get; set; }
        public int GwclAreaId { get; set; }
        public int GwclRegionId { get; set; }
        public virtual GwclRegion GwclRegion { get; set; }
    }
}
