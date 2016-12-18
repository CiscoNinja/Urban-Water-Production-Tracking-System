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
            RuleFor(production => production.OptionId).NotNull()
                .WithMessage("Select a water option");

            RuleFor(production => production.OptionTypeId).NotNull()
                .WithMessage("Select an option type");

            RuleFor(production => production.WSystemId).NotNull()
                .WithMessage("Select a system");

            RuleFor(production => production.DailyActual).GreaterThanOrEqualTo(0)
                .WithMessage("please enter a value");

            RuleFor(production => production.FRPH).GreaterThanOrEqualTo(0)
                .WithMessage("please enter a value");

            RuleFor(production => production.FRPS).GreaterThanOrEqualTo(0)
                .WithMessage("please enter a value");

            RuleFor(production => production.TFPD).GreaterThanOrEqualTo(0)
                .WithMessage("please enter a value");

            RuleFor(production => production.NTFPD).NotNull()
                .WithMessage("please enter a value");

            RuleFor(production => production.LOG).GreaterThanOrEqualTo(0)
                .WithMessage("please enter a value");

            RuleFor(production => production.DateCreated).NotNull()
                .WithMessage("please select today's date");

            RuleFor(production => production.DayToRecord).NotNull()
               .WithMessage("please select date");
        }
    }
}