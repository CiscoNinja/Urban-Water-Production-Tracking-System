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
    [Authorize(Roles = "Super")]
    [RoutePrefix("api/gwclregions")]
    public class GwclRegionsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<GwclRegion> _gwclregionsRepository;
        private readonly IEntityBaseRepository<GwclArea> _gwclareasRepository;
        private readonly IEntityBaseRepository<AreaRegion> _arearegionRepository;

        public GwclRegionsController(IEntityBaseRepository<GwclRegion> gwclregionsRepository,
             IEntityBaseRepository<GwclArea> gwclareasRepository, IEntityBaseRepository<AreaRegion> arearegionRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _gwclregionsRepository = gwclregionsRepository;
            _gwclareasRepository = gwclareasRepository;
            _arearegionRepository = arearegionRepository;
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

        [AllowAnonymous]
        [Route("loadregions")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
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

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var gwclregions = _gwclregionsRepository.GetAll()
                    .Where(c => c.Name.ToLower().Contains(filter) ||
                    c.Code.ToLower().Contains(filter)).ToList();

                var gwclregionsVm = Mapper.Map<IEnumerable<GwclRegion>, IEnumerable<GwclRegionViewModel>>(gwclregions);

                response = request.CreateResponse<IEnumerable<GwclRegionViewModel>>(HttpStatusCode.OK, gwclregionsVm);

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
                var gwclregion = _gwclregionsRepository.GetSingle(id);

                GwclRegionViewModel gwclregionVm = Mapper.Map<GwclRegion, GwclRegionViewModel>(gwclregion);

                response = request.CreateResponse<GwclRegionViewModel>(HttpStatusCode.OK, gwclregionVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, GwclRegionViewModel gwclregion)
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
                    if (_gwclregionsRepository.RegionExists(gwclregion.Name, gwclregion.Code))
                    {
                        ModelState.AddModelError("Invalid area", "Name or Code already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        GwclRegion newRegion = new GwclRegion();
                        newRegion.UpdateRegion(gwclregion);
                        _gwclregionsRepository.Add(newRegion);

                        _unitOfWork.Commit();

                        addRegionToArea(newRegion, gwclregion.GwclAreaID);

                        _unitOfWork.Commit();
                        // Update view model
                        gwclregion = Mapper.Map<GwclRegion, GwclRegionViewModel>(newRegion);
                        response = request.CreateResponse<GwclRegionViewModel>(HttpStatusCode.Created, gwclregion);
                    }
                }

                return response;
            });
        }

        private void addRegionToArea(GwclRegion gwclregion, int areaId)
        {
            var gwclarea = _gwclareasRepository.GetSingle(areaId);
            if (gwclarea == null)
                throw new ApplicationException("Area doesn't exist.");

            var regionArea = new AreaRegion()
            {
                GwclAreaId = gwclarea.ID,
                GwclRegionId = gwclregion.ID
            };
            _arearegionRepository.Add(regionArea);
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, GwclRegionViewModel gwclregion)
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
                    GwclRegion _gwclregion = _gwclregionsRepository.GetSingle(gwclregion.ID);
                    _gwclregion.UpdateRegion(gwclregion);

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
                List<GwclRegion> gwclregions = null;
                int totalRegions = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    gwclregions = _gwclregionsRepository.FindBy(c => c.Name.ToLower().Contains(filter) ||
                            c.Code.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalRegions = _gwclregionsRepository.GetAll()
                        .Where(c => c.Name.ToLower().Contains(filter) ||
                            c.Code.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    gwclregions = _gwclregionsRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalRegions = _gwclregionsRepository.GetAll().Count();
                }

                IEnumerable<GwclRegionViewModel> gwclregionsVM = Mapper.Map<IEnumerable<GwclRegion>, IEnumerable<GwclRegionViewModel>>(gwclregions);

                PaginationSet<GwclRegionViewModel> pagedSet = new PaginationSet<GwclRegionViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalRegions,
                    TotalPages = (int)Math.Ceiling((decimal)totalRegions / currentPageSize),
                    Items = gwclregionsVM
                };

                response = request.CreateResponse<PaginationSet<GwclRegionViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}