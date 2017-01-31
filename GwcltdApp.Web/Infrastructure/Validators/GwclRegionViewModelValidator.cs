using FluentValidation;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Validators
{
    public class GwclRegionViewModelValidator : AbstractValidator<GwclRegionViewModel>
    {
        public GwclRegionViewModelValidator()
        {
            RuleFor(gwclregion => gwclregion.Name).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(gwclregion => gwclregion.Code).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(gwclregion => gwclregion.GwclAreaID).NotNull()
                .WithMessage("Select Area");
        }
    }
}