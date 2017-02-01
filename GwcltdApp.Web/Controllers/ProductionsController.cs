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
    [RoutePrefix("api/productions")]
    public class ProductionsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Production> _productionsRepository;

        public ProductionsController(IEntityBaseRepository<Production> productionsRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _productionsRepository = productionsRepository;
        }

        [AllowAnonymous]
        [Route("latest/{userstation:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int userstation)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var productions = _productionsRepository.GetAll().Where(s => s.GwclStationId == userstation).OrderByDescending(m => m.DayToRecord).Take(4).ToList();

                IEnumerable<ProductionViewModel> productionsVM = Mapper.Map<IEnumerable<Production>, IEnumerable<ProductionViewModel>>(productions);

                response = request.CreateResponse<IEnumerable<ProductionViewModel>>(HttpStatusCode.OK, productionsVM);

                return response;
            });
        }

        //[AllowAnonymous]
        [Route("summary/{userstation:int}/{id}")]
        public HttpResponseMessage GetTable(HttpRequestMessage request, int userstation, string id)
        {
            var allsys = SummaryManager.GetAllUserSystems(userstation);//change underying code to get systems from the logged in user's station
            List<GraphData> list = new List<GraphData>();
            var graph = new MyDictionary();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var allProductions = _productionsRepository.GetAll();
                switch (id)
                {
                    case "Plant Losses1":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.PlantLoss_metre(item.Code, 1);
                                var febTotal = SummaryManager.PlantLoss_metre(item.Code, 2);
                                var marchTotal = SummaryManager.PlantLoss_metre(item.Code, 3);
                                var aprilTotal = SummaryManager.PlantLoss_metre(item.Code, 4);
                                var mayTotal = SummaryManager.PlantLoss_metre(item.Code, 5);
                                var juneTotal = SummaryManager.PlantLoss_metre(item.Code, 6);
                                var july7Total = SummaryManager.PlantLoss_metre(item.Code, 7);
                                var augTotal = SummaryManager.PlantLoss_metre(item.Code, 8);
                                var septTotal = SummaryManager.PlantLoss_metre(item.Code, 9);
                                var octTotal = SummaryManager.PlantLoss_metre(item.Code, 10);
                                var novTotal = SummaryManager.PlantLoss_metre(item.Code, 11);
                                var decTotal = SummaryManager.PlantLoss_metre(item.Code, 12);
                                graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                            }

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Plant Losses2":
                        {
                            foreach (var item in allsys)
                            {
                                //string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);
                                var janTotal = SummaryManager.PlantLoss_percent(item.Code, 1);
                                var febTotal = SummaryManager.PlantLoss_percent(item.Code, 2);
                                var marchTotal = SummaryManager.PlantLoss_percent(item.Code, 3);
                                var aprilTotal = SummaryManager.PlantLoss_percent(item.Code, 4);
                                var mayTotal = SummaryManager.PlantLoss_percent(item.Code, 5);
                                var juneTotal = SummaryManager.PlantLoss_percent(item.Code, 6);
                                var july7Total = SummaryManager.PlantLoss_percent(item.Code, 7);
                                var augTotal = SummaryManager.PlantLoss_percent(item.Code, 8);
                                var septTotal = SummaryManager.PlantLoss_percent(item.Code, 9);
                                var octTotal = SummaryManager.PlantLoss_percent(item.Code, 10);
                                var novTotal = SummaryManager.PlantLoss_percent(item.Code, 11);
                                var decTotal = SummaryManager.PlantLoss_percent(item.Code, 12);
                                graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                            }

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Daily Average":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getDailyAverage(item.Code, 1);
                                var febTotal = SummaryManager.getDailyAverage(item.Code, 2);
                                var marchTotal = SummaryManager.getDailyAverage(item.Code, 3);
                                var aprilTotal = SummaryManager.getDailyAverage(item.Code, 4);
                                var mayTotal = SummaryManager.getDailyAverage(item.Code, 5);
                                var juneTotal = SummaryManager.getDailyAverage(item.Code, 6);
                                var july7Total = SummaryManager.getDailyAverage(item.Code, 7);
                                var augTotal = SummaryManager.getDailyAverage(item.Code, 8);
                                var septTotal = SummaryManager.getDailyAverage(item.Code, 9);
                                var octTotal = SummaryManager.getDailyAverage(item.Code, 10);
                                var novTotal = SummaryManager.getDailyAverage(item.Code, 11);
                                var decTotal = SummaryManager.getDailyAverage(item.Code, 12);
                                graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                            }

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Plant Capacity":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getPlantCap(item.Code, 1, item.Code);
                                var febTotal = SummaryManager.getPlantCap(item.Code, 2, item.Code);
                                var marchTotal = SummaryManager.getPlantCap(item.Code, 3, item.Code);
                                var aprilTotal = SummaryManager.getPlantCap(item.Code, 4, item.Code);
                                var mayTotal = SummaryManager.getPlantCap(item.Code, 5, item.Code);
                                var juneTotal = SummaryManager.getPlantCap(item.Code, 6, item.Code);
                                var july7Total = SummaryManager.getPlantCap(item.Code, 7, item.Code);
                                var augTotal = SummaryManager.getPlantCap(item.Code, 8, item.Code);
                                var septTotal = SummaryManager.getPlantCap(item.Code, 9, item.Code);
                                var octTotal = SummaryManager.getPlantCap(item.Code, 10, item.Code);
                                var novTotal = SummaryManager.getPlantCap(item.Code, 11, item.Code);
                                var decTotal = SummaryManager.getPlantCap(item.Code, 12, item.Code);
                                graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                            }

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Service Standard":
                        {
                            break;
                        }
                    case "Hours Loss(M\xB3)":
                        {
                            break;
                        }
                    case "Treated Water":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getWaterTable(item.Code, 1, "Treated Water");
                                var febTotal = SummaryManager.getWaterTable(item.Code, 2, "Treated Water");
                                var marchTotal = SummaryManager.getWaterTable(item.Code, 3, "Treated Water");
                                var aprilTotal = SummaryManager.getWaterTable(item.Code, 4, "Treated Water");
                                var mayTotal = SummaryManager.getWaterTable(item.Code, 5, "Treated Water");
                                var juneTotal = SummaryManager.getWaterTable(item.Code, 6, "Treated Water");
                                var july7Total = SummaryManager.getWaterTable(item.Code, 7, "Treated Water");
                                var augTotal = SummaryManager.getWaterTable(item.Code, 8, "Treated Water");
                                var septTotal = SummaryManager.getWaterTable(item.Code, 9, "Treated Water");
                                var octTotal = SummaryManager.getWaterTable(item.Code, 10, "Treated Water");
                                var novTotal = SummaryManager.getWaterTable(item.Code, 11, "Treated Water");
                                var decTotal = SummaryManager.getWaterTable(item.Code, 12, "Treated Water");
                                graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                            }

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Raw Water":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getWaterTable(item.Code, 1, "Raw Water");
                                var febTotal = SummaryManager.getWaterTable(item.Code, 2, "Raw Water");
                                var marchTotal = SummaryManager.getWaterTable(item.Code, 3, "Raw Water");
                                var aprilTotal = SummaryManager.getWaterTable(item.Code, 4, "Raw Water");
                                var mayTotal = SummaryManager.getWaterTable(item.Code, 5, "Raw Water");
                                var juneTotal = SummaryManager.getWaterTable(item.Code, 6, "Raw Water");
                                var july7Total = SummaryManager.getWaterTable(item.Code, 7, "Raw Water");
                                var augTotal = SummaryManager.getWaterTable(item.Code, 8, "Raw Water");
                                var septTotal = SummaryManager.getWaterTable(item.Code, 9, "Raw Water");
                                var octTotal = SummaryManager.getWaterTable(item.Code, 10, "Raw Water");
                                var novTotal = SummaryManager.getWaterTable(item.Code, 11, "Raw Water");
                                var decTotal = SummaryManager.getWaterTable(item.Code, 12, "Raw Water");
                                graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                            }

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    default:
                        {
                            response = request.CreateResponse<string>(HttpStatusCode.OK, null);
                            break;
                        }
                }
                

                return response;
            });
        }

        //[AllowAnonymous]
        [Route("charts/{id}")]
        public HttpResponseMessage GetChart(HttpRequestMessage request, string id)
        {
            List<ChartData> list = new List<ChartData>();
            var graph = new MyDictionary1();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
               // var allProductions = _productionsRepository.GetAll();

                switch(id)
                {
                    case "Treated Water":
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                DateTime monthvalue = new DateTime(2016, i, 1);
                                string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);

                                var system1Total = SummaryManager.getWaterTable("S01",i,"Treated Water");
                                var system2Total = SummaryManager.getWaterTable("S02", i, "Treated Water");
                                var system3Total = SummaryManager.getWaterTable("S03", i, "Treated Water");
                                var system4Total = SummaryManager.getWaterTable("S04", i, "Treated Water");
                                var system5Total = SummaryManager.getWaterTable("S05", i, "Treated Water");
                                var system6Total = SummaryManager.getWaterTable("S06", i, "Treated Water");
                                var system7Total = SummaryManager.getWaterTable("S07", i, "Treated Water");
                                graph.Add(thismonth, system1Total, system2Total, system3Total, system4Total, system5Total, system6Total, system7Total);
                            }

                            response = request.CreateResponse<MyDictionary1>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Plant Loss":
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                DateTime monthvalue = new DateTime(2016, i, 1);
                                string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);

                                var system1Total = SummaryManager.PlantLoss_percent("S01", i);
                                var system2Total = SummaryManager.PlantLoss_percent("S02", i);
                                var system3Total = SummaryManager.PlantLoss_percent("S03", i);
                                var system4Total = SummaryManager.PlantLoss_percent("S04", i);
                                var system5Total = SummaryManager.PlantLoss_percent("S05", i);
                                var system6Total = SummaryManager.PlantLoss_percent("S06", i);
                                var system7Total = SummaryManager.PlantLoss_percent("S07", i);
                                graph.Add(thismonth, system1Total, system2Total, system3Total, system4Total, system5Total, system6Total, system7Total);
                            }

                            response = request.CreateResponse<MyDictionary1>(HttpStatusCode.OK, graph);
                            break;
                        }
                        
                    default:
                        {
                            response = request.CreateResponse<string>(HttpStatusCode.OK, null);
                            break;
                        }
                }
                return response;
            });
        }

        public struct GraphData
        {
            public string systemName { get; set; }
            public double Jan { get; set; }
            public double Feb { get; set; }
            public double March { get; set; }
            public double April { get; set; }
            public double May { get; set; }
            public double June { get; set; }
            public double July { get; set; }
            public double Aug { get; set; }
            public double Sept { get; set; }
            public double Oct { get; set; }
            public double Nov { get; set; }
            public double Dec { get; set; }
        }

        public struct ChartData
        {
            public string month { get; set; }
            public double S01 { get; set; }
            public double S02 { get; set; }
            public double S03 { get; set; }
            public double S04 { get; set; }
            public double S05 { get; set; }
            public double S06 { get; set; }
            public double S07 { get; set; }
        }

        public class MyDictionary1 : List<ChartData>
        {
            public void Add(string key, double value1, double value2, double value3, double value4, double value5, double value6, double value7)
            {
                ChartData val = new ChartData();
                val.month = key;
                val.S01 = value1;
                val.S02 = value2;
                val.S03 = value3;
                val.S04 = value4;
                val.S05 = value5;
                val.S06 = value6;
                val.S07 = value7;
                this.Add(val);
            }
        }
        public class MyDictionary : List<GraphData>
        {
            public void Add(string key, double value1, double value2, double value3, double value4, double value5, double value6, double value7, double value8, double value9, double value10, double value11, double value12)
            {
                GraphData val = new GraphData();
                val.systemName = key;
                val.Jan = value1;
                val.Feb = value2;
                val.March = value3;
                val.April = value4;
                val.May = value5;
                val.June = value6;
                val.July = value7;
                val.Aug = value8;
                val.Sept = value9;
                val.Oct = value10;
                val.Nov = value11;
                val.Dec = value12;
                this.Add(val);
            }
        }
        [Route("details/{id:int}")]
        public HttpResponseMessage GetSingle(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var production = _productionsRepository.GetSingle(id);

                ProductionViewModel productionVM = Mapper.Map<Production, ProductionViewModel>(production);

                response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.OK, productionVM);

                return response;
            });
        }

        //[AllowAnonymous]
        [Route("{userstation:int}/{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int userstation, int? page, int? pageSize, DateTime? filter1 = null, DateTime? filter2 = null, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Production> productions = null;
                int totalProductions = new int();

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
                if (filter1.HasValue && filter2.HasValue && string.IsNullOrEmpty(filter) )
                {//change code to get systems from the logged in user's station
                    productions = _productionsRepository.GetAll()
                        .Where(m => m.GwclStationId == userstation && (DbFunctions.TruncateTime(m.DayToRecord) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.DayToRecord) <= DbFunctions.TruncateTime(filter2.Value)))
                        .OrderBy(m => m.DayToRecord)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalProductions = _productionsRepository.GetAll()
                        .Where(m => m.GwclStationId == userstation && (DbFunctions.TruncateTime(m.DayToRecord) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.DayToRecord) <= DbFunctions.TruncateTime(filter2.Value)))
                        .Count();
                }
                else
                {
                    productions = _productionsRepository
                        .GetAll().Where(m => m.GwclStationId == userstation)
                        .OrderBy(m => m.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalProductions = _productionsRepository.GetAll().Where(m => m.GwclStationId == userstation).Count();
                }

                IEnumerable<ProductionViewModel> productionsVM = Mapper.Map<IEnumerable<Production>, IEnumerable<ProductionViewModel>>(productions);

                PaginationSet<ProductionViewModel> pagedSet = new PaginationSet<ProductionViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalProductions,
                    TotalPages = (int)Math.Ceiling((decimal)totalProductions / currentPageSize),
                    Items = productionsVM
                };

                response = request.CreateResponse<PaginationSet<ProductionViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, ProductionViewModel production)
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
                    Production newProduction = new Production();
                    newProduction.UpdateProduction(production);

                    
                    _productionsRepository.Add(newProduction);

                    _unitOfWork.Commit();

                    // Update view model
                    production = Mapper.Map<Production, ProductionViewModel>(newProduction);
                    response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.Created, production);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductionViewModel production)
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
                    var productionDb = _productionsRepository.GetSingle(production.ID);
                    if (productionDb == null)
                        response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid production.");
                    else
                    {
                        productionDb.UpdateProduction(production);
                        _productionsRepository.Edit(productionDb);

                        _unitOfWork.Commit();
                        response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.OK, production);
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