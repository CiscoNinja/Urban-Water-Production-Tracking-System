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
    [RoutePrefix("api/gwclstations")]
    public class GwclStationsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<GwclStation> _gwclstationsRepository;
        private readonly IEntityBaseRepository<GwclRegion> _gwclregionsRepository;
        private readonly IEntityBaseRepository<RegionStation> _regionstationsRepository;

        public GwclStationsController(IEntityBaseRepository<GwclStation> gwclstationsRepository,
            IEntityBaseRepository<GwclRegion> gwclregionsRepository, IEntityBaseRepository<RegionStation> regionstationsRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _gwclstationsRepository = gwclstationsRepository;
            _gwclregionsRepository = gwclregionsRepository;
            _regionstationsRepository = regionstationsRepository;
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

        [AllowAnonymous]
        [Route("loadstations")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
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

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var gwclstations = _gwclstationsRepository.GetAll()
                    .Where(c => c.Name.ToLower().Contains(filter) ||
                    c.StationCode.ToLower().Contains(filter)).ToList();

                var gwclstationsVm = Mapper.Map<IEnumerable<GwclStation>, IEnumerable<GwclStationViewModel>>(gwclstations);

                response = request.CreateResponse<IEnumerable<GwclStationViewModel>>(HttpStatusCode.OK, gwclstationsVm);

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
                var gwclstations = _gwclstationsRepository.GetSingle(id);

                GwclStationViewModel gwclstationVm = Mapper.Map<GwclStation, GwclStationViewModel>(gwclstations);

                response = request.CreateResponse<GwclStationViewModel>(HttpStatusCode.OK, gwclstationVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, GwclStationViewModel gwclstation)
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
                    if (_gwclstationsRepository.StationExists(gwclstation.Name, gwclstation.StationCode))
                    {
                        ModelState.AddModelError("Invalid area", "Name or Code already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        GwclStation newWStation = new GwclStation();
                        newWStation.UpdateStation(gwclstation);
                        _gwclstationsRepository.Add(newWStation);

                        _unitOfWork.Commit();

                        addStationToArea(newWStation, gwclstation.GwclRegionId);

                        _unitOfWork.Commit();
                        // Update view model
                        gwclstation = Mapper.Map<GwclStation, GwclStationViewModel  >(newWStation);
                        response = request.CreateResponse<GwclStationViewModel>(HttpStatusCode.Created, gwclstation);
                    }
                }

                return response;
            });
        }

        private void addStationToArea(GwclStation gwclstation, int regionId)
        {
            var gwclregion = _gwclregionsRepository.GetSingle(regionId);
            if (gwclregion == null)
                throw new ApplicationException("Region doesn't exist.");

            var stationRegion = new RegionStation()
            {
                GwclStationId = gwclstation.ID,
                GwclRegionId = gwclregion.ID
            };
            _regionstationsRepository.Add(stationRegion);
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, GwclStationViewModel gwclstation)
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
                    GwclStation _gwclstation = _gwclstationsRepository.GetSingle(gwclstation.ID);
                    _gwclstation.UpdateStation(gwclstation);

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
                List<GwclStation> gwclstations = null;
                int totalStations = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    gwclstations = _gwclstationsRepository.FindBy(c => c.Name.ToLower().Contains(filter) ||
                            c.StationCode.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalStations = _gwclstationsRepository.GetAll()
                        .Where(c => c.Name.ToLower().Contains(filter) ||
                            c.StationCode.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    gwclstations = _gwclstationsRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalStations = _gwclstationsRepository.GetAll().Count();
                }

                IEnumerable<GwclStationViewModel> gwclstationsVM = Mapper.Map<IEnumerable<GwclStation>, IEnumerable<GwclStationViewModel>>(gwclstations);

                PaginationSet<GwclStationViewModel> pagedSet = new PaginationSet<GwclStationViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalStations,
                    TotalPages = (int)Math.Ceiling((decimal)totalStations / currentPageSize),
                    Items = gwclstationsVM
                };

                response = request.CreateResponse<PaginationSet<GwclStationViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}