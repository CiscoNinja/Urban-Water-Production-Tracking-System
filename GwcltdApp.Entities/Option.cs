using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class Option : IEntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string OptionOf { get; set; }
    }
}
