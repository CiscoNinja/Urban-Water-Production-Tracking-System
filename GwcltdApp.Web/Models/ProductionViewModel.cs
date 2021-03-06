using GwcltdApp.Web.Infrastructure.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GwcltdApp.Web.Models
{
    public class ProductionViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; } //date of recording
        public DateTime DayToRecord { get; set; } //day value being recorded
        public double DailyActual { get; set; }
        public string Comment { get; set; }
        public double FRPH { get; set; }//flow rate per hour
        public double FRPS { get; set; }//flow rate per second
        public double TFPD { get; set; }//total flow per day
        public double NTFPD { get; set; }//negative total flow per day
        public double LOG { get; set; }
        public string WSystem { get; set; }
        public string WSystemCode { get; set; }
        public int WSystemId { get; set; }
        public string Option { get; set; }
        public int OptionId { get; set; }
        public string OptionType { get; set; }
        public int OptionTypeId { get; set; }
        public int GwclStationId { get; set; }
        public string StationCode { get; set; }
        public string GwclStation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ProductionViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}