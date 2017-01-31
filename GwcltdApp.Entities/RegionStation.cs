using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class RegionStation : IEntityBase
    {
        public int ID { get; set; }
        public int GwclRegionId { get; set; }
        public int GwclStationId { get; set; }
        public virtual GwclStation GwclStation { get; set; }
    }
}
