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
    [RoutePrefix("api/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Role> _rolesRepository;

        public RolesController(IEntityBaseRepository<Role> rolesRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _rolesRepository = rolesRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var roles = _rolesRepository.GetAll().ToList();

                IEnumerable<RoleViewModel> rolesVM = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(roles);

                response = request.CreateResponse<IEnumerable<RoleViewModel>>(HttpStatusCode.OK, rolesVM);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("loadroles")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var roles = _rolesRepository.GetAll().ToList();

                IEnumerable<RoleViewModel> rolesVM = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(roles);

                response = request.CreateResponse<IEnumerable<RoleViewModel>>(HttpStatusCode.OK, rolesVM);

                return response;
            });
        }
    }
}