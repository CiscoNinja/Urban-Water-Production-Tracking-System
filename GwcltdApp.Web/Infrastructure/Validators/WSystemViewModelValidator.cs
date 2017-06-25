using FluentValidation;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Validators
{
    public class WSystemViewModelValidator : AbstractValidator<WSystemViewModel>
    {
        public WSystemViewModelValidator()
        {
            RuleFor(ws => ws.Name).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(ws => ws.Code).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(ws => ws.Capacity).GreaterThanOrEqualTo(0)
                .WithMessage("please enter a value");

            RuleFor(ws => ws.GwclStationId).NotNull()
               .WithMessage("Select Area");
        }
    }
}