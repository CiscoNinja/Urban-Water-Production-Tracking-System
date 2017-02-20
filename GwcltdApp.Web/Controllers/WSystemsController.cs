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
    [RoutePrefix("api/gwclsystems")]
    public class WSystemsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<WSystem> _wsystemsRepository;
        private readonly IEntityBaseRepository<GwclStation> _gwclstationsRepository;
        private readonly IEntityBaseRepository<StationSystem> _stationsystemRepository;

        public WSystemsController(IEntityBaseRepository<WSystem> wsystemsRepository,
            IEntityBaseRepository<GwclStation> gwclstationsRepository, IEntityBaseRepository<StationSystem> stationsystemRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _wsystemsRepository = wsystemsRepository;
            _gwclstationsRepository = gwclstationsRepository;
            _stationsystemRepository = stationsystemRepository;
        }

        [AllowAnonymous]
        [Route("loadsystems/{stationid:int}")]
        public HttpResponseMessage Getsys(HttpRequestMessage request, int stationid)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var wsystems = _wsystemsRepository.GetAll().Where(x => x.GwclStationId == stationid).ToList();

                IEnumerable<WSystemViewModel> wsystemsVM = Mapper.Map<IEnumerable<WSystem>, IEnumerable<WSystemViewModel>>(wsystems);

                response = request.CreateResponse<IEnumerable<WSystemViewModel>>(HttpStatusCode.OK, wsystemsVM);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("syscodes/{stationid:int}")]
        public HttpResponseMessage GetCodes(HttpRequestMessage request, int stationid)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var wsystems = _wsystemsRepository.GetAll().Where(x => x.GwclStationId == stationid).Select(x => x.Code).ToArray();

                //IEnumerable<WSystemViewModel> wsystemsVM = Mapper.Map<IEnumerable<WSystem>, IEnumerable<WSystemViewModel>>(wsystems);

                response = request.CreateResponse(HttpStatusCode.OK, wsystems);

                return response;
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var gwclsystems = _wsystemsRepository.GetAll()
                    .Where(c => c.Name.ToLower().Contains(filter) ||
                    c.Code.ToLower().Contains(filter)).ToList();

                var gwclsystemsVm = Mapper.Map<IEnumerable<WSystem>, IEnumerable<WSystemViewModel>>(gwclsystems);

                response = request.CreateResponse<IEnumerable<WSystemViewModel>>(HttpStatusCode.OK, gwclsystemsVm);

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
                var gwclsystem = _wsystemsRepository.GetSingle(id);

                WSystemViewModel gwclsystemVm = Mapper.Map<WSystem, WSystemViewModel>(gwclsystem);

                response = request.CreateResponse<WSystemViewModel>(HttpStatusCode.OK, gwclsystemVm);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, WSystemViewModel gwclsystem)
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
                    if (_wsystemsRepository.SystemExists(gwclsystem.Name, gwclsystem.Code))
                    {
                        ModelState.AddModelError("Invalid area", "Name or Code already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        WSystem newSystem = new WSystem();
                        newSystem.UpdateSystem(gwclsystem);
                        _wsystemsRepository.Add(newSystem);

                        _unitOfWork.Commit();

                        addSystemToStation(newSystem, gwclsystem.GwclStationId);

                        _unitOfWork.Commit();
                        // Update view model
                        gwclsystem = Mapper.Map<WSystem, WSystemViewModel>(newSystem);
                        response = request.CreateResponse<WSystemViewModel>(HttpStatusCode.Created, gwclsystem);
                    }
                }

                return response;
            });
        }

        private void addSystemToStation(WSystem gwclsystem, int stationId)
        {
            var gwclstation = _gwclstationsRepository.GetSingle(stationId);
            if (gwclstation == null)
                throw new ApplicationException("Station doesn't exist.");

            var systemStation = new StationSystem()
            {
                WSystemID = gwclsystem.ID,
                GwclStationId = gwclstation.ID
            };
            _stationsystemRepository.Add(systemStation);
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, WSystemViewModel gwclsystem)
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
                    WSystem _gwclsystem = _wsystemsRepository.GetSingle(gwclsystem.ID);
                    _gwclsystem.UpdateSystem(gwclsystem);

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
                List<WSystem> gwclsystems = null;
                int totalSystems = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    gwclsystems = _wsystemsRepository.FindBy(c => c.Name.ToLower().Contains(filter) ||
                            c.Code.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalSystems = _wsystemsRepository.GetAll()
                        .Where(c => c.Name.ToLower().Contains(filter) ||
                            c.Code.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    gwclsystems = _wsystemsRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalSystems = _wsystemsRepository.GetAll().Count();
                }

                IEnumerable<WSystemViewModel> gwclsystemsVM = Mapper.Map<IEnumerable<WSystem>, IEnumerable<WSystemViewModel>>(gwclsystems);

                PaginationSet<WSystemViewModel> pagedSet = new PaginationSet<WSystemViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalSystems,
                    TotalPages = (int)Math.Ceiling((decimal)totalSystems / currentPageSize),
                    Items = gwclsystemsVM
                };

                response = request.CreateResponse<PaginationSet<WSystemViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}
