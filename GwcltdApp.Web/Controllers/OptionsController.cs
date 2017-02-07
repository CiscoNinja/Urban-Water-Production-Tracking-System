using AutoMapper;
using GwcltdApp.Data.Extensions;
using GwcltdApp.Data.Infrastructure;
using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using GwcltdApp.Web.Infrastructure.Core;
using GwcltdApp.Web.Infrastructure.Extensions;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GwcltdApp.Web.Controllers
{
    [Authorize(Roles = "Super, Admin")]
    [RoutePrefix("api/gwcloptions")]
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

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var gwcloptions = _optionsRepository.GetAll()
                    .Where(c => c.Name.ToLower().Contains(filter) ||
                    c.OptionOf.ToLower().Contains(filter)).ToList();

                var gwcloptionsVm = Mapper.Map<IEnumerable<Option>, IEnumerable<OptionViewModel>>(gwcloptions);

                response = request.CreateResponse<IEnumerable<OptionViewModel>>(HttpStatusCode.OK, gwcloptionsVm);

                return response;
            });
        }

        [HttpGet]
        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var gwcloption = _optionsRepository.GetSingle(id);

                OptionViewModel gwcloptionVm = Mapper.Map<Option, OptionViewModel>(gwcloption);

                response = request.CreateResponse<OptionViewModel>(HttpStatusCode.OK, gwcloptionVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, OptionViewModel gwcloption)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                }
                else
                {
                    if (_optionsRepository.OptionExists(gwcloption.Name))
                    {
                        ModelState.AddModelError("Invalid option", "Name already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Option newOption = new Option();
                        newOption.UpdateOption(gwcloption);
                        _optionsRepository.Add(newOption);

                        _unitOfWork.Commit();

                        // Update view model
                        gwcloption = Mapper.Map<Option, OptionViewModel>(newOption);
                        response = request.CreateResponse<OptionViewModel>(HttpStatusCode.Created, gwcloption);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, OptionViewModel gwcloption)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                }
                else
                {
                    Option _gwcloption = _optionsRepository.GetSingle(gwcloption.ID);
                    _gwcloption.UpdateOption(gwcloption);

                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpGet]
        [Route("{page:int=0}/{pageSize=4}/{filter?}")]
        public HttpResponseMessage Search(HttpRequestMessage request, int? page, int? pageSize, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Option> gwcloptions = null;
                int totalOptions = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    gwcloptions = _optionsRepository.FindBy(c => c.Name.ToLower().Contains(filter) ||
                            c.OptionOf.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalOptions = _optionsRepository.GetAll()
                        .Where(c => c.Name.ToLower().Contains(filter) ||
                            c.OptionOf.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    gwcloptions = _optionsRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalOptions = _optionsRepository.GetAll().Count();
                }

                IEnumerable<OptionViewModel> gwclareasVM = Mapper.Map<IEnumerable<Option>, IEnumerable<OptionViewModel>>(gwcloptions);

                PaginationSet<OptionViewModel> pagedSet = new PaginationSet<OptionViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalOptions,
                    TotalPages = (int)Math.Ceiling((decimal)totalOptions / currentPageSize),
                    Items = gwclareasVM
                };

                response = request.CreateResponse<PaginationSet<OptionViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}
