using AutoMapper;
using GwcltdApp.Data.Infrastructure;
using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using GwcltdApp.Web.Infrastructure.Core;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GwcltdApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/optiontypes")]
    public class OptionTypesController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<OptionType> _optiontypesRepository;

        public OptionTypesController(IEntityBaseRepository<OptionType> optiontypesRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _optiontypesRepository = optiontypesRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var optiontypes = _optiontypesRepository.GetAll().ToList();

                IEnumerable<OptionTypeViewModel> optiontypesVM = Mapper.Map<IEnumerable<OptionType>, IEnumerable<OptionTypeViewModel>>(optiontypes);

                response = request.CreateResponse<IEnumerable<OptionTypeViewModel>>(HttpStatusCode.OK, optiontypesVM);

                return response;
            });
        }
    }
}
