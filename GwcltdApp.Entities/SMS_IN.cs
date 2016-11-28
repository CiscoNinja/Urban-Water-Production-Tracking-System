using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class SMS_IN : IEntityBase
    {
        public int ID { get; set; }
        public string SMS_TEXT { get; set; }
        public string SENDER_NUMBER { get; set; }
        public DateTime SENT_DT { get; set; }
        public DateTime TS { get; set; }
    }
}
