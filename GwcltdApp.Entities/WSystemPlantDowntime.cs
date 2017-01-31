using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class WSystemPlantDowntime : IEntityBase
    {
        public int ID { get; set; }
        public int WSystemId { get; set; }
        public int PlantDowntimeId { get; set; }
        public virtual PlantDowntime PlantDowntime { get; set; }
    }
}
