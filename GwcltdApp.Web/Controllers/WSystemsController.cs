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
    [RoutePrefix("api/wsystems")]
    public class WSystemsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<WSystem> _wsystemsRepository;

        public WSystemsController(IEntityBaseRepository<WSystem> wsystemsRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _wsystemsRepository = wsystemsRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var wsystems = _wsystemsRepository.GetAll().ToList();

                IEnumerable<WSystemViewModel> wsystemsVM = Mapper.Map<IEnumerable<WSystem>, IEnumerable<WSystemViewModel>>(wsystems);

                response = request.CreateResponse<IEnumerable<WSystemViewModel>>(HttpStatusCode.OK, wsystemsVM);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("syscodes")]
        public HttpResponseMessage GetCodes(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var wsystems = _wsystemsRepository.GetAll().Select(x => x.Code).ToArray();

                //IEnumerable<WSystemViewModel> wsystemsVM = Mapper.Map<IEnumerable<WSystem>, IEnumerable<WSystemViewModel>>(wsystems);

                response = request.CreateResponse(HttpStatusCode.OK, wsystems);

                return response;
            });
        }
    }
}
