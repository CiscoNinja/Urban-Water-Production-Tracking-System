using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class GwclStation : IEntityBase
    {
        public GwclStation()
        {
            StationSystems = new List<StationSystem>();
            UserStations = new List<UserStation>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string StationCode { get; set; }
        public int GwclRegionId { get; set; }
        public virtual GwclRegion GwclRegion { get; set; }
        public virtual ICollection<StationSystem> StationSystems { get; set; }
        public virtual ICollection<UserStation> UserStations { get; set; }
    }
}
