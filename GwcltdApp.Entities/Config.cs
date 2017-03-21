using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class Config : IEntityBase
    {
        public int ID { get; set; }
        public string ConfigData { get; set; }
    }
}
