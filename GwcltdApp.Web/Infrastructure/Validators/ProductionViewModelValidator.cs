using FluentValidation;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Validators
{
    public class ProductionViewModelValidator : AbstractValidator<ProductionViewModel>
    {
        public ProductionViewModelValidator()
        {
            RuleFor(production => production.OptionId).GreaterThan(0)
                .WithMessage("Select a water option");

            RuleFor(production => production.OptionTypeId).GreaterThan(0)
                .WithMessage("Select an option type");

            RuleFor(production => production.WSystemId).GreaterThan(0)
                .WithMessage("Select a system");

            RuleFor(production => production.DailyActual).GreaterThan(0)
                .WithMessage("please enter a value");

            RuleFor(production => production.DateCreated).NotNull()
                .WithMessage("please select a today's date");

            RuleFor(production => production.DayToRecord).NotNull()
               .WithMessage("please select date");
        }
    }
}