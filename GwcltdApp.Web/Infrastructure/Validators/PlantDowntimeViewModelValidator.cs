using FluentValidation;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Validators
{
    public class PlantDowntimeViewModelValidator : AbstractValidator<PlantDowntimeViewModel>
    {
        public PlantDowntimeViewModelValidator()
        {
            RuleFor(downtime => downtime.CurrentDate).NotNull()
                .WithMessage("Select a date");

            RuleFor(downtime => downtime.EndTime).NotNull()
                .WithMessage("Select a date");

            RuleFor(downtime => downtime.HoursDown).NotNull();

            RuleFor(downtime => downtime.Starttime).NotNull()
                .WithMessage("Select a date");

            RuleFor(production => production.WSystemId).GreaterThanOrEqualTo(0)
                .WithMessage("please select a value");
        }
    }
}