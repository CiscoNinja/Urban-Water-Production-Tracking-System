using FluentValidation;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Validators
{
    public class GwclStationViewModelValidator : AbstractValidator<GwclStationViewModel>
    {
        public GwclStationViewModelValidator()
        {
            RuleFor(gsvm => gsvm.Name).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(gsvm => gsvm.Code).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(gsvm => gsvm.GwclRegionId).NotNull()
                .WithMessage("Select Area");
        }
    }
}