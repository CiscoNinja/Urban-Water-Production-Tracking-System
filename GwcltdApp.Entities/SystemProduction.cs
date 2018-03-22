using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class SystemProduction : IEntityBase
    {
        public int ID { get; set; }
        public int WSystemId { get; set; }
        public int HourlyProductionId { get; set; }
        public virtual HourlyProduction HourlyProduction { get; set; }
    }
}
