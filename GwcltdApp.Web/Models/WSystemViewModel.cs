using GwcltdApp.Web.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Models
{
    public class WSystemViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; } //system code
        public string Name { get; set; } //system name
        public int Capacity { get; set; }
        public int GwclStationId { get; set; }
        public string GwclStation { get; set; }
        
         public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new WSystemViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}