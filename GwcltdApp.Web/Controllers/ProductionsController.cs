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
    [Authorize(Roles = "Admin")]
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
        [Route("latest")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var productions = _productionsRepository.GetAll().OrderByDescending(m => m.DayToRecord).Take(4).ToList();

                IEnumerable<ProductionViewModel> productionsVM = Mapper.Map<IEnumerable<Production>, IEnumerable<ProductionViewModel>>(productions);

                response = request.CreateResponse<IEnumerable<ProductionViewModel>>(HttpStatusCode.OK, productionsVM);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("summary/{id}")]
        public HttpResponseMessage GetTable(HttpRequestMessage request, string id)
        {
            var allsys = SummaryManager.GetAllSystems();
            List<GraphData> list = new List<GraphData>();
            var graph = new MyDictionary();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                foreach (var item in allsys)
                {
                    //string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);

                    var janTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 1 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var febTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 2 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var marchTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 3 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var aprilTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 4 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var mayTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 5 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var juneTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 6 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var july7Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 7 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var augTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 8 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var septTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 9 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var octTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 10 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var novTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 11 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    var decTotal = _productionsRepository.GetAll().Where(x => x.WSystem.Code == item.Code && x.DayToRecord.Month == 12 && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    graph.Add(item.Name + " (" + item.Code + ")", janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                }

                response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);

                return response;
            });
        }

        [AllowAnonymous]
        [Route("charts/{id}")]
        public HttpResponseMessage GetChart(HttpRequestMessage request, string id)
        {
            List<ChartData> list = new List<ChartData>();
            var graph = new MyDictionary1();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                for (int i = 1; i <= 12; i++)
                {
                    DateTime monthvalue = new DateTime(2016, i, 1);
                    string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);

                    int system1Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S01" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    int system2Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S02" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    int system3Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S03" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    int system4Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S04" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    int system5Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S05" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    int system6Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S06" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    int system7Total = _productionsRepository.GetAll().Where(x => x.WSystem.Code == "S07" && x.DayToRecord.Month == monthvalue.Month && x.Option.Name == id).Select(x => x.DailyActual).DefaultIfEmpty().Sum();
                    graph.Add(thismonth, system1Total, system2Total, system3Total, system4Total, system5Total, system6Total, system7Total);
                }

                response = request.CreateResponse<MyDictionary1>(HttpStatusCode.OK, graph);

                return response;
            });
        }

        public struct GraphData
        {
            public string systemName { get; set; }
            public int Jan { get; set; }
            public int Feb { get; set; }
            public int March { get; set; }
            public int April { get; set; }
            public int May { get; set; }
            public int June { get; set; }
            public int July { get; set; }
            public int Aug { get; set; }
            public int Sept { get; set; }
            public int Oct { get; set; }
            public int Nov { get; set; }
            public int Dec { get; set; }
        }

        public struct ChartData
        {
            public string month { get; set; }
            public int S01 { get; set; }
            public int S02 { get; set; }
            public int S03 { get; set; }
            public int S04 { get; set; }
            public int S05 { get; set; }
            public int S06 { get; set; }
            public int S07 { get; set; }
        }

        public class MyDictionary1 : List<ChartData>
        {
            public void Add(string key, int value1, int value2, int value3, int value4, int value5, int value6, int value7)
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
            public void Add(string key, int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8, int value9, int value10, int value11, int value12)
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
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
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

        [AllowAnonymous]
        [Route("{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page, int? pageSize, DateTime? filter1 = null, DateTime? filter2 = null, string filter = null)
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
                {
                    productions = _productionsRepository.GetAll()
                        .Where(m => DbFunctions.TruncateTime(m.DayToRecord) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.DayToRecord) <= DbFunctions.TruncateTime(filter2.Value))
                        .OrderBy(m => m.DayToRecord)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalProductions = _productionsRepository.GetAll()
                        .Where(m => DbFunctions.TruncateTime(m.DayToRecord) >= DbFunctions.TruncateTime(filter1.Value) && DbFunctions.TruncateTime(m.DayToRecord) <= DbFunctions.TruncateTime(filter2.Value))
                        .Count();
                }
                else
                {
                    productions = _productionsRepository
                        .GetAll()
                        .OrderBy(m => m.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalProductions = _productionsRepository.GetAll().Count();
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