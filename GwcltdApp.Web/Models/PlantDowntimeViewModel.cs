using GwcltdApp.Web.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Models
{
    public class PlantDowntimeViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime Starttime { get; set; }
        public DateTime EndTime { get; set; }
        public int HoursDown { get; set; }
        public int WSystemId { get; set; }
        public string WSystem { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PlantDowntimeViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}