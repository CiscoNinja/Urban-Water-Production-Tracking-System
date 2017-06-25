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
using System.Web;
using System.Web.Http;

namespace GwcltdApp.Web.Controllers
{
    [Authorize(Roles = "Super, Admin")]
    [RoutePrefix("api/gwclareas")]
    public class GwclAreasController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<GwclArea> _gwclareaRepository;

        public GwclAreasController(IEntityBaseRepository<GwclArea> gwclareaRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _gwclareaRepository = gwclareaRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var gwclareas = _gwclareaRepository.GetAll().ToList();

                IEnumerable<GwclAreaViewModel> gwclareasVM = Mapper.Map<IEnumerable<GwclArea>, IEnumerable<GwclAreaViewModel>>(gwclareas);

                response = request.CreateResponse<IEnumerable<GwclAreaViewModel>>(HttpStatusCode.OK, gwclareasVM);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("loadareas")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var gwclareas = _gwclareaRepository.GetAll().ToList();

                IEnumerable<GwclAreaViewModel> gwclareasVM = Mapper.Map<IEnumerable<GwclArea>, IEnumerable<GwclAreaViewModel>>(gwclareas);

                response = request.CreateResponse<IEnumerable<GwclAreaViewModel>>(HttpStatusCode.OK, gwclareasVM);

                return response;
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var gwclareas = _gwclareaRepository.GetAll()
                    .Where(c => c.Name.ToLower().Contains(filter) ||
                    c.Code.ToLower().Contains(filter)).ToList();

                var gwclareasVm = Mapper.Map<IEnumerable<GwclArea>, IEnumerable<GwclAreaViewModel>>(gwclareas);

                response = request.CreateResponse<IEnumerable<GwclAreaViewModel>>(HttpStatusCode.OK, gwclareasVm);

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
                var warea = _gwclareaRepository.GetSingle(id);

                GwclAreaViewModel wareaVm = Mapper.Map<GwclArea, GwclAreaViewModel>(warea);

                response = request.CreateResponse<GwclAreaViewModel>(HttpStatusCode.OK, wareaVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, GwclAreaViewModel warea)
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
                    if (_gwclareaRepository.AreaExists(warea.Name, warea.Code))
                    {
                        ModelState.AddModelError("Invalid area", "Name or Code already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        GwclArea newWArea = new GwclArea();
                        newWArea.UpdateWArea(warea);
                        _gwclareaRepository.Add(newWArea);

                        _unitOfWork.Commit();

                        // Update view model
                        warea = Mapper.Map<GwclArea, GwclAreaViewModel>(newWArea);
                        response = request.CreateResponse<GwclAreaViewModel>(HttpStatusCode.Created, warea);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, GwclAreaViewModel warea)
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
                    GwclArea _warea = _gwclareaRepository.GetSingle(warea.ID);
                    _warea.UpdateWArea(warea);

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
                List<GwclArea> gwclareas = null;
                int totalWAreas = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    gwclareas = _gwclareaRepository.FindBy(c => c.Name.ToLower().Contains(filter) ||
                            c.Code.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalWAreas = _gwclareaRepository.GetAll()
                        .Where(c => c.Name.ToLower().Contains(filter) ||
                            c.Code.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    gwclareas = _gwclareaRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalWAreas = _gwclareaRepository.GetAll().Count();
                }

                IEnumerable<GwclAreaViewModel> gwclareasVM = Mapper.Map<IEnumerable<GwclArea>, IEnumerable<GwclAreaViewModel>>(gwclareas);

                PaginationSet<GwclAreaViewModel> pagedSet = new PaginationSet<GwclAreaViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalWAreas,
                    TotalPages = (int)Math.Ceiling((decimal)totalWAreas / currentPageSize),
                    Items = gwclareasVM
                };

                response = request.CreateResponse<PaginationSet<GwclAreaViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}