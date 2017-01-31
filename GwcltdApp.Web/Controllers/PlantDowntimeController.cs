using AutoMapper;
using GwcltdApp.Data.Infrastructure;
using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using GwcltdApp.Web.Infrastructure.Core;
using GwcltdApp.Web.Infrastructure.Extensions;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace GwcltdApp.Web.Controllers
{
    [Authorize(Roles = "Super")]
    [RoutePrefix("api/plantdowntime")]
    public class PlantDowntimeController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<PlantDowntime> _plantdowntimeRepository;

        public PlantDowntimeController(IEntityBaseRepository<PlantDowntime> plantdowntimeRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _plantdowntimeRepository = plantdowntimeRepository;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var plantdowntime = _plantdowntimeRepository.GetAll().ToList();

                IEnumerable<PlantDowntimeViewModel> plantdowntimeVM = Mapper.Map<IEnumerable<PlantDowntime>, IEnumerable<PlantDowntimeViewModel>>(plantdowntime);

                response = request.CreateResponse<IEnumerable<PlantDowntimeViewModel>>(HttpStatusCode.OK, plantdowntimeVM);

                return response;
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var plantdowntime = _plantdowntimeRepository.GetSingle(id);

                PlantDowntimeViewModel plantdowntimesVM = Mapper.Map<PlantDowntime, PlantDowntimeViewModel>(plantdowntime);

                response = request.CreateResponse<PlantDowntimeViewModel>(HttpStatusCode.OK, plantdowntimesVM);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page, int? pageSize, DateTime? filter1 = null, DateTime? filter2 = null, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<PlantDowntime> plantdowntimes = null;
                int totalPlantdowntimes = new int();
                if (filter1.HasValue && filter2.HasValue && string.IsNullOrEmpty(filter))
                {
                    plantdowntimes = _plantdowntimeRepository.GetAll()
                        .Where(m => DbFunctions.TruncateTime(m.CurrentDate) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.CurrentDate) <= DbFunctions.TruncateTime(filter2.Value))
                        .OrderBy(m => m.CurrentDate)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalPlantdowntimes = _plantdowntimeRepository.GetAll()
                        .Where(m => DbFunctions.TruncateTime(m.CurrentDate) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.CurrentDate) <= DbFunctions.TruncateTime(filter2.Value))
                        .Count();
                }
                else
                {
                    plantdowntimes = _plantdowntimeRepository
                        .GetAll()
                        .OrderBy(m => m.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalPlantdowntimes = _plantdowntimeRepository.GetAll().Count();
                }

                IEnumerable<PlantDowntimeViewModel> plantdowntimesVM = Mapper.Map<IEnumerable<PlantDowntime>, IEnumerable<PlantDowntimeViewModel>>(plantdowntimes);

                PaginationSet<PlantDowntimeViewModel> pagedSet = new PaginationSet<PlantDowntimeViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalPlantdowntimes,
                    TotalPages = (int)Math.Ceiling((decimal)totalPlantdowntimes / currentPageSize),
                    Items = plantdowntimesVM
                };

                response = request.CreateResponse<PaginationSet<PlantDowntimeViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, PlantDowntimeViewModel plantdowntime)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    PlantDowntime newPlantDowntime = new PlantDowntime();
                    newPlantDowntime.UpdatePlantDowntime(plantdowntime);


                    _plantdowntimeRepository.Add(newPlantDowntime);

                    _unitOfWork.Commit();

                    // Update view model
                    plantdowntime = Mapper.Map<PlantDowntime, PlantDowntimeViewModel>(newPlantDowntime);
                    response = request.CreateResponse<PlantDowntimeViewModel>(HttpStatusCode.Created, plantdowntime);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, PlantDowntimeViewModel plantdowntime)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var plantdowntimeDb = _plantdowntimeRepository.GetSingle(plantdowntime.ID);
                    if (plantdowntimeDb == null)
                        response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid plant downtime.");
                    else
                    {
                        plantdowntimeDb.UpdatePlantDowntime(plantdowntime);
                        _plantdowntimeRepository.Edit(plantdowntimeDb);

                        _unitOfWork.Commit();
                        response = request.CreateResponse<PlantDowntimeViewModel>(HttpStatusCode.OK, plantdowntime);
                    }
                }

                return response;
            });
        }
    }
}