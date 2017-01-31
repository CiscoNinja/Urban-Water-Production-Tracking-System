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
using System.Web;
using System.Web.Mvc;

namespace GwcltdApp.Web.Controllers
{
    [Authorize(Roles = "Super")]
    [RoutePrefix("api/gwclstations")]
    public class GwclStationsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<GwclStation> _gwclstationsRepository;

        public GwclStationsController(IEntityBaseRepository<GwclStation> gwclstationsRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _gwclstationsRepository = gwclstationsRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var gwclstations = _gwclstationsRepository.GetAll().ToList();

                IEnumerable<GwclStationViewModel> gwclstationsVM = Mapper.Map<IEnumerable<GwclStation>, IEnumerable<GwclStationViewModel>>(gwclstations);

                response = request.CreateResponse<IEnumerable<GwclStationViewModel>>(HttpStatusCode.OK, gwclstationsVM);

                return response;
            });
        }
    }
}