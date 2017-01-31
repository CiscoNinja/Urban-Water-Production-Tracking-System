using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class GwclRegion : IEntityBase
    {
        public GwclRegion()
        {
            RegionStations = new List<RegionStation>();
            UserRegions = new List<UserRegion>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int GwclAreaID { get; set; }
        public virtual GwclArea GwclArea { get; set; }
        public virtual ICollection<UserRegion> UserRegions { get; set; }
        public virtual ICollection<RegionStation> RegionStations { get; set; }
    }
}
