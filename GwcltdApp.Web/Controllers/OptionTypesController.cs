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
    [RoutePrefix("api/gwclotypes")]
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

        [AllowAnonymous]
        [Route("loadoptypes")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
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
        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var gwclotypes = _optiontypesRepository.GetAll()
                    .Where(c => c.Name.ToLower().Contains(filter)).ToList();

                var gwclotypesVm = Mapper.Map<IEnumerable<OptionType>, IEnumerable<OptionTypeViewModel>>(gwclotypes);

                response = request.CreateResponse<IEnumerable<OptionTypeViewModel>>(HttpStatusCode.OK, gwclotypesVm);

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
                var gwclotype = _optiontypesRepository.GetSingle(id);

                OptionTypeViewModel gwclotypeVm = Mapper.Map<OptionType, OptionTypeViewModel>(gwclotype);

                response = request.CreateResponse<OptionTypeViewModel>(HttpStatusCode.OK, gwclotypeVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, OptionTypeViewModel gwclotype)
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
                    if (_optiontypesRepository.TypeExists(gwclotype.Name))
                    {
                        ModelState.AddModelError("Invalid area", "Name already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        OptionType newType = new OptionType();
                        newType.UpdateType(gwclotype);
                        _optiontypesRepository.Add(newType);

                        _unitOfWork.Commit();

                        // Update view model
                        gwclotype = Mapper.Map<OptionType, OptionTypeViewModel>(newType);
                        response = request.CreateResponse<OptionTypeViewModel>(HttpStatusCode.Created, gwclotype);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, OptionTypeViewModel gwclotype)
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
                    OptionType _gwclotype = _optiontypesRepository.GetSingle(gwclotype.ID);
                    _gwclotype.UpdateType(gwclotype);

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
                List<OptionType> gwclotypes = null;
                int totalTypes = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    gwclotypes = _optiontypesRepository.FindBy(c => c.Name.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalTypes = _optiontypesRepository.GetAll()
                        .Where(c => c.Name.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    gwclotypes = _optiontypesRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalTypes = _optiontypesRepository.GetAll().Count();
                }

                IEnumerable<OptionTypeViewModel> gwclotypesVM = Mapper.Map<IEnumerable<OptionType>, IEnumerable<OptionTypeViewModel>>(gwclotypes);

                PaginationSet<OptionTypeViewModel> pagedSet = new PaginationSet<OptionTypeViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalTypes,
                    TotalPages = (int)Math.Ceiling((decimal)totalTypes / currentPageSize),
                    Items = gwclotypesVM
                };

                response = request.CreateResponse<PaginationSet<OptionTypeViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}
