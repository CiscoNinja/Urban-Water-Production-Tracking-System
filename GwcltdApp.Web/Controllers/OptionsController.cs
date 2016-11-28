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
    [RoutePrefix("api/options")]
    public class OptionsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Option> _optionsRepository;

        public OptionsController(IEntityBaseRepository<Option> optionsRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _optionsRepository = optionsRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var options = _optionsRepository.GetAll().ToList();

                IEnumerable<OptionViewModel> optionsVM = Mapper.Map<IEnumerable<Option>, IEnumerable<OptionViewModel>>(options);

                response = request.CreateResponse<IEnumerable<OptionViewModel>>(HttpStatusCode.OK, optionsVM);

                return response;
            });
        }
    }
}
