using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class PlantDowntime : IEntityBase
    {
        public int ID { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime Starttime { get; set; }
        public DateTime EndTime { get; set; }
        public int HoursDown { get; set; }
        public int WSystemId { get; set; }
        public virtual WSystem WSystem { get; set; }
    }
}
