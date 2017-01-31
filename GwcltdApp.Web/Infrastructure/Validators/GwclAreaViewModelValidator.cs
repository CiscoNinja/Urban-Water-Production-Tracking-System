using FluentValidation;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Validators
{
    public class GwclAreaViewModelValidator : AbstractValidator<GwclAreaViewModel>
    {
        public GwclAreaViewModelValidator()
        {
            RuleFor(gwclarea => gwclarea.Name).NotEmpty()
                .WithMessage("please enter a value");

            RuleFor(gwclarea => gwclarea.Code).NotEmpty()
                .WithMessage("please enter a value");
        }
    }
}