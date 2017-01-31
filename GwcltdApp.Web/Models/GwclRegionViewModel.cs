using GwcltdApp.Web.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Models
{
    public class GwclRegionViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int GwclAreaID { get; set; }
        public string GwclArea { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new GwclRegionViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}