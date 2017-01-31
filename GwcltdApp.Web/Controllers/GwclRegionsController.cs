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
    [Authorize(Roles = "Super")]
    [RoutePrefix("api/gwclregions")]
    public class GwclRegionsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<GwclRegion> _gwclregionsRepository;

        public GwclRegionsController(IEntityBaseRepository<GwclRegion> gwclregionsRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _gwclregionsRepository = gwclregionsRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var gwclregions = _gwclregionsRepository.GetAll().ToList();

                IEnumerable<GwclRegionViewModel> gwclregionsVM = Mapper.Map<IEnumerable<GwclRegion>, IEnumerable<GwclRegionViewModel>>(gwclregions);

                response = request.CreateResponse<IEnumerable<GwclRegionViewModel>>(HttpStatusCode.OK, gwclregionsVM);

                return response;
            });
        }
    }
}