using GwcltdApp.Web.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Models
{
    public class GwclStationViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StationCode { get; set; }
        public int GwclRegionId { get; set; }
        public string GwclRegion  { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new GwclStationViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}