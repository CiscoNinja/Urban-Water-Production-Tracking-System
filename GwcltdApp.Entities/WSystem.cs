using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    /// <summary>
    /// GwcltdApp Movie Stock Availability
    /// </summary>
    public class WSystem : IEntityBase
    {
        public int ID { get; set; }
        public string Code { get; set; } //system code
        public string Name { get; set; } //system name
    }
}
