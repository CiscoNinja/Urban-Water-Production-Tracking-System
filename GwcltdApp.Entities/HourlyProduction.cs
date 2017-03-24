using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    public class HourlyProduction : IEntityBase
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; } //date of recording
        public DateTime DayToRecord { get; set; } //day value being recorded
        public double HourlyActual { get; set; }
        public string Comment { get; set; }
        public double FRPH { get; set; }//flow rate per hour
        public double FRPS { get; set; }//flow rate per second
        public double TFPD { get; set; }//total flow per day
        public double NTFPD { get; set; }//negative total flow per day
        public double LOG { get; set; }
        public int WSystemId { get; set; }
        public int OptionId { get; set; }
        public int OptionTypeId { get; set; }
        public virtual WSystem WSystem { get; set; }
        public virtual Option Option { get; set; }
        public virtual OptionType OptionType { get; set; }
        public int GwclStationId { get; set; }
        public virtual GwclStation GwclStation { get; set; }
    }
}
