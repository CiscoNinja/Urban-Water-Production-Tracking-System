using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    /// <summary>
    /// GwcltdApp Rental Info
    /// </summary>
    public class Production : IEntityBase
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; } //date of recording
        public DateTime DayToRecord { get; set; } //day value being recorded
        public int DailyActual { get; set; }
        public string Comment { get; set; }
        public int FRPH { get; set; }//flow rate per hour
        public int FRPS { get; set; }//flow rate per second
        public int TFPD { get; set; }//total flow per day
        public int NTFPD { get; set; }//negative total flow per day
        public int LOG { get; set; }
        public int WSystemId { get; set; }
        public int OptionId { get; set; }
        public int OptionTypeId { get; set; }
        public virtual WSystem WSystem { get; set; }
        public virtual Option Option { get; set; }
        public virtual OptionType OptionType { get; set; }
    }
}
