using AutoMapper;
using GwcltdApp.Data.Infrastructure;
using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using GwcltdApp.Web.Infrastructure.Core;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GwcltdApp.Web.Infrastructure.Extensions;
using GwcltdApp.Data.Extensions;
using GwcltdApp.Web.DAL;
using System.Globalization;
using System.Data.Entity;

namespace GwcltdApp.Web.Controllers
{
    [Authorize(Roles = "Super, Admin, User")]
    [RoutePrefix("api/hrlyproductions")]
    public class HourlyProductionsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<HourlyProduction> _hrlyproductionsRepository;
        private readonly IEntityBaseRepository<Production> _newproductionsRepository;

        public HourlyProductionsController(IEntityBaseRepository<HourlyProduction> hrlyproductionsRepository,
            IEntityBaseRepository<Production> newproductionsRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _hrlyproductionsRepository = hrlyproductionsRepository;
            _newproductionsRepository = newproductionsRepository;
        }
        
        [Route("latest/{userstation:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int userstation)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var hrlyproductions = _hrlyproductionsRepository.GetAll().Where(s => s.GwclStationId == userstation).OrderByDescending(m => m.DayToRecord).Take(4).ToList();

                IEnumerable<ProductionViewModel> hrlyproductionsVM = Mapper.Map<IEnumerable<HourlyProduction>, IEnumerable<ProductionViewModel>>(hrlyproductions);

                response = request.CreateResponse<IEnumerable<ProductionViewModel>>(HttpStatusCode.OK, hrlyproductionsVM);

                return response;
            });
        }

       
        [Route("details/{id:int}")]
        public HttpResponseMessage GetSingle(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var hrlyproduction = _hrlyproductionsRepository.GetSingle(id);

                ProductionViewModel hrlyproductionVM = Mapper.Map<HourlyProduction, ProductionViewModel>(hrlyproduction);

                response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.OK, hrlyproductionVM);

                return response;
            });
        }
        
        [Route("{userstation:int}/{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int userstation, int? page, int? pageSize, DateTime? filter1 = null, DateTime? filter2 = null, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<HourlyProduction> hrlyproductions = null;
                int totalhrlyProductions = new int();

                //if (!string.IsNullOrEmpty(filter) && !filter1.HasValue && !filter2.HasValue)
                //{
                //    productions = _productionsRepository.GetAll()
                //        .Where(m => m.WSystem.Code.ToLower().Contains(filter.ToLower().Trim())
                //        || m.WSystem.Name.ToLower().Contains(filter.ToLower().Trim())
                //        || m.Option.Name.ToLower().Contains(filter.ToLower().Trim())
                //        || m.OptionType.Name.ToLower().Contains(filter.ToLower().Trim()))
                //        .OrderBy(m => m.DayToRecord)
                //        .Skip(currentPage * currentPageSize)
                //        .Take(currentPageSize)
                //        .ToList();

                //    totalProductions = _productionsRepository.GetAll()
                //        .Where(m => m.WSystem.Code.ToLower().Contains(filter.ToLower().Trim())
                //        || m.WSystem.Name.ToLower().Contains(filter.ToLower().Trim())
                //        || m.Option.Name.ToLower().Contains(filter.ToLower().Trim())
                //        || m.OptionType.Name.ToLower().Contains(filter.ToLower().Trim()))
                //        .Count();
                //}
                //else 
                if (filter1.HasValue && filter2.HasValue && string.IsNullOrEmpty(filter))
                {//change code to get systems from the logged in user's station
                    hrlyproductions = _hrlyproductionsRepository.GetAll()
                        .Where(m => m.GwclStationId == userstation && (DbFunctions.TruncateTime(m.DayToRecord) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.DayToRecord) <= DbFunctions.TruncateTime(filter2.Value)))
                        .OrderBy(m => m.DayToRecord)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalhrlyProductions = _hrlyproductionsRepository.GetAll()
                        .Where(m => m.GwclStationId == userstation && (DbFunctions.TruncateTime(m.DayToRecord) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.DayToRecord) <= DbFunctions.TruncateTime(filter2.Value)))
                        .Count();
                }
                else
                {
                    hrlyproductions = _hrlyproductionsRepository
                        .GetAll().Where(m => m.GwclStationId == userstation)
                        .OrderBy(m => m.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalhrlyProductions = _hrlyproductionsRepository.GetAll().Where(m => m.GwclStationId == userstation).Count();
                }

                IEnumerable<ProductionViewModel> hrlyproductionsVM = Mapper.Map<IEnumerable<HourlyProduction>, IEnumerable<ProductionViewModel>>(hrlyproductions);

                PaginationSet<ProductionViewModel> pagedSet = new PaginationSet<ProductionViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalhrlyProductions,
                    TotalPages = (int)Math.Ceiling((decimal)totalhrlyProductions / currentPageSize),
                    Items = hrlyproductionsVM
                };

                response = request.CreateResponse<PaginationSet<ProductionViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, ProductionViewModel hrlyproduction)
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
                    HourlyProduction newhrlyProduction = new HourlyProduction();
                    newhrlyProduction.UpdateHrlyProduction(hrlyproduction);


                    _hrlyproductionsRepository.Add(newhrlyProduction);

                    _unitOfWork.Commit();

                    DateTime firstHour = Convert.ToDateTime("1/1/2017 1:00:00 AM");
                    if (Convert.ToDateTime(hrlyproduction.DayToRecord).Hour.Equals(firstHour.Hour))
                    {
                        Production newproduction = new Production();
                        var currenttotal = hrlyproduction.TFPD;
                        var prviousdate = hrlyproduction.DayToRecord.AddDays(-1);
                        var dayActual = SummaryManager.DailyActual(currenttotal, prviousdate, hrlyproduction.WSystemId, hrlyproduction.OptionId);
                        hrlyproduction.DailyActual = dayActual;
                        newproduction.UpdateProduction(hrlyproduction);

                        _newproductionsRepository.Add(newproduction);

                        _unitOfWork.Commit();
                    }

                    // Update view model
                    hrlyproduction = Mapper.Map<HourlyProduction, ProductionViewModel>(newhrlyProduction);
                    response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.Created, hrlyproduction);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductionViewModel hrlyproduction)
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
                    var hrlyproductionDb = _hrlyproductionsRepository.GetSingle(hrlyproduction.ID);
                    if (hrlyproductionDb == null)
                        response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid production.");
                    else
                    {
                        hrlyproductionDb.UpdateHrlyProduction(hrlyproduction);
                        _hrlyproductionsRepository.Edit(hrlyproductionDb);

                        _unitOfWork.Commit();

                        DateTime firstHour = Convert.ToDateTime("1/1/2017 1:00:00 AM");
                        if (Convert.ToDateTime(hrlyproduction.DayToRecord).Hour.Equals(firstHour.Hour))
                        {
                            var newproductionDb = _newproductionsRepository.GetAll().FirstOrDefault(x => x.DayToRecord == hrlyproduction.DayToRecord);
                            if (newproductionDb == null)
                            { }
                            else
                            {
                                var currenttotal = hrlyproduction.TFPD;
                                var prviousdate = hrlyproduction.DayToRecord.AddDays(-1);
                                hrlyproduction.DailyActual = SummaryManager.DailyActual(currenttotal, prviousdate, hrlyproduction.WSystemId, hrlyproduction.OptionId);
                                newproductionDb.UpdateProduction(hrlyproduction);

                                _newproductionsRepository.Edit(newproductionDb);

                                _unitOfWork.Commit();
                            };
                        }

                        response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.OK, hrlyproduction);
                    }
                }

                return response;
            });
        }

        //[MimeMultipart]
        //[Route("images/upload")]
        //public HttpResponseMessage Post(HttpRequestMessage request, int movieId)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;

        //        var movieOld = _productionsRepository.GetSingle(movieId);
        //        if (movieOld == null)
        //            response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid movie.");
        //        else
        //        {
        //            var uploadPath = HttpContext.Current.Server.MapPath("~/Content/images/movies");

        //            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

        //            // Read the MIME multipart asynchronously 
        //            Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

        //            string _localFileName = multipartFormDataStreamProvider
        //                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

        //            // Create response
        //            FileUploadResult fileUploadResult = new FileUploadResult
        //            {
        //                LocalFilePath = _localFileName,

        //                FileName = Path.GetFileName(_localFileName),

        //                FileLength = new FileInfo(_localFileName).Length
        //            };

        //            // update database
        //            movieOld.Image = fileUploadResult.FileName;
        //            _productionsRepository.Edit(movieOld);
        //            _unitOfWork.Commit();

        //            response = request.CreateResponse(HttpStatusCode.OK, fileUploadResult);
        //        }

        //        return response;
        //    });
        //}
    }
}