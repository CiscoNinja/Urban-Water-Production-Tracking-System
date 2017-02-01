using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class WSystem : IEntityBase
    {
        public WSystem()
        {
            SystemProductions = new List<SystemProduction>();
            WSystemPlantDowntimes = new List<WSystemPlantDowntime>();
        }

        public int ID { get; set; }
        public string Code { get; set; } //system code
        public string Name { get; set; } //system name
        public int Capacity { get; set; }//plant capacity
        public int GwclStationId { get; set; }
        public virtual GwclStation GwclStation { get; set; }
        public virtual ICollection<SystemProduction> SystemProductions { get; set; }
        public virtual ICollection<WSystemPlantDowntime> WSystemPlantDowntimes { get; set; }
    }
}
