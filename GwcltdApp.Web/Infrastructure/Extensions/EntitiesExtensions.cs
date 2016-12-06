using GwcltdApp.Entities;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Infrastructure.Extensions
{
    public static class EntitiesExtensions
    {
        public static void UpdateProduction(this Production production, ProductionViewModel productionVm)
        {
            production.Comment = productionVm.Comment;
            production.DailyActual = productionVm.DailyActual;
            production.DateCreated = productionVm.DateCreated;
            production.DayToRecord = productionVm.DayToRecord;
            production.FRPH = productionVm.FRPH;
            production.FRPS = productionVm.FRPS;
            production.TFPD = productionVm.TFPD;
            production.NTFPD = productionVm.NTFPD;
            production.OptionId = productionVm.OptionId;
            production.OptionTypeId = productionVm.OptionTypeId;
            production.WSystemId = productionVm.WSystemId;
        }

        //public static void UpdateCustomer(this Customer customer, CustomerViewModel customerVm)
        //{
        //    customer.FirstName = customerVm.FirstName;
        //    customer.LastName = customerVm.LastName;
        //    customer.IdentityCard = customerVm.IdentityCard;
        //    customer.Mobile = customerVm.Mobile;
        //    customer.DateOfBirth = customerVm.DateOfBirth;
        //    customer.Email = customerVm.Email;
        //    customer.UniqueKey = (customerVm.UniqueKey == null || customerVm.UniqueKey == Guid.Empty)
        //        ? Guid.NewGuid() : customerVm.UniqueKey;
        //    customer.RegistrationDate = (customer.RegistrationDate == DateTime.MinValue ? DateTime.Now : customerVm.RegistrationDate);
        //}
    }
}