using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class StationSystem : IEntityBase
    {
        public int ID { get; set; }
        public int GwclStationId { get; set; }
        public int WSystemID { get; set; }
        public virtual WSystem WSystem { get; set; }
    }
}
