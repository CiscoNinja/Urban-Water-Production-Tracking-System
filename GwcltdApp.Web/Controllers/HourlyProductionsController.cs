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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using GwcltdApp.Web.viewModel;

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
        [Route("importExcel")]
        public HttpResponseMessage ImportFromExcel(HttpRequestMessage request, ExcelViewModel hrlyproduction)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            //int rpositn = 0;
            object[,] rawWater = null;
            object[,] TrtdWater = null;
            string[] xrmonths = { "E6", "AI6", "E19", "AF19", "E32", "AI32", "E45", "AH45", "E58", "AI58" }; // starting and ending cells of raw water on excel sheet
            string[] xtmonths = { "E12", "AI12", "E25", "AF25", "E38", "AI38", "E51", "AH51", "E64", "AI64" }; // starting and ending cells of treated water on excel sheet

            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;

                ProductionViewModel newViewModel = new ProductionViewModel();
                HourlyProduction newhrlyProduction = new HourlyProduction();
                Production newproduction = new Production();
                //string fullpath = Path.GetFullPath(fileName);
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@hrlyproduction.filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item("S01");

                int cmonth = 0;
                int rday;
                int tday;
                double rTFPD = 0;
                double tTFPD = 0;

                for(int valPos = 0; valPos < 10; valPos+=2)
                {
                    rday = 1;
                    tday = 1;
                    cmonth++;
                    int nextpost = valPos + 1;
                    Excel.Range rRange = xlWorkSheet.Range[xlWorkSheet.Range[xrmonths[valPos]], xlWorkSheet.Range[xrmonths[nextpost]]];
                    Excel.Range tRange = xlWorkSheet.Range[xlWorkSheet.Range[xtmonths[valPos]], xlWorkSheet.Range[xtmonths[nextpost]]];
                    //double rangeVal = rRange.Cells.Value2;
                    rawWater = rRange.Value2;
                    TrtdWater = tRange.Value2;

                    foreach (double r in rawWater)
                    {
                        rTFPD += r;
                        newViewModel.Comment = hrlyproduction.Comment;
                        newViewModel.DailyActual = r;
                        newViewModel.DateCreated = Convert.ToDateTime((cmonth + "/" + rday + "/2017 01:00 am").ToString());
                        newViewModel.DayToRecord = Convert.ToDateTime((cmonth + "/" + rday + "/2017 01:00 am").ToString());
                        newViewModel.FRPH = hrlyproduction.FRPH;
                        newViewModel.FRPS = hrlyproduction.FRPS;
                        newViewModel.GwclStation = hrlyproduction.GwclStation;
                        newViewModel.GwclStationId = hrlyproduction.GwclStationId;
                        newViewModel.LOG = hrlyproduction.LOG;
                        newViewModel.NTFPD = hrlyproduction.NTFPD;
                        newViewModel.Option = hrlyproduction.Option;
                        newViewModel.OptionId = 14;
                        newViewModel.OptionType = hrlyproduction.OptionType;
                        newViewModel.OptionTypeId = 21;
                        newViewModel.StationCode = hrlyproduction.StationCode;
                        newViewModel.TFPD = rTFPD;
                        newViewModel.WSystem = hrlyproduction.WSystem;
                        newViewModel.WSystemCode = hrlyproduction.WSystemCode;
                        newViewModel.WSystemId = hrlyproduction.WSystemId;

                        newhrlyProduction.UpdateHrlyProduction(newViewModel);
                        newproduction.UpdateProduction(newViewModel);

                        _newproductionsRepository.Add(newproduction);
                        _hrlyproductionsRepository.Add(newhrlyProduction);

                        _unitOfWork.Commit();

                        rday++;
                    }

                    foreach (double t in TrtdWater)
                    {
                        tTFPD += t;
                        newViewModel.Comment = hrlyproduction.Comment;
                        newViewModel.DailyActual = t;
                        newViewModel.DateCreated = Convert.ToDateTime((cmonth + "/" + tday + "/2017 01:00 am").ToString());
                        newViewModel.DayToRecord = Convert.ToDateTime((cmonth + "/" + tday + "/2017 01:00 am").ToString());
                        newViewModel.FRPH = hrlyproduction.FRPH;
                        newViewModel.FRPS = hrlyproduction.FRPS;
                        newViewModel.GwclStation = hrlyproduction.GwclStation;
                        newViewModel.GwclStationId = hrlyproduction.GwclStationId;
                        newViewModel.LOG = hrlyproduction.LOG;
                        newViewModel.NTFPD = hrlyproduction.NTFPD;
                        newViewModel.Option = hrlyproduction.Option;
                        newViewModel.OptionId = 13;
                        newViewModel.OptionType = hrlyproduction.OptionType;
                        newViewModel.OptionTypeId = 17;
                        newViewModel.StationCode = hrlyproduction.StationCode;
                        newViewModel.TFPD = tTFPD;
                        newViewModel.WSystem = hrlyproduction.WSystem;
                        newViewModel.WSystemCode = hrlyproduction.WSystemCode;
                        newViewModel.WSystemId = hrlyproduction.WSystemId;

                        newproduction.UpdateProduction(newViewModel);
                        newhrlyProduction.UpdateHrlyProduction(newViewModel);

                        _newproductionsRepository.Add(newproduction);
                        _hrlyproductionsRepository.Add(newhrlyProduction);

                        _unitOfWork.Commit();

                        tday++;
                    }
                    
                }

                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);

                // Update view model
                newViewModel = Mapper.Map<HourlyProduction, ProductionViewModel>(newhrlyProduction);
                response = request.CreateResponse<ProductionViewModel>(HttpStatusCode.Created, newViewModel);


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


        double[] ConvertToDoubleArray(System.Array values)
        {

            // create a new string array
            double[] theArray = new double[values.Length];

            // loop through the 2-D System.Array and populate the 1-D String Array
            for (int i = 1; i <= values.Length; i++)
            {
                if (values.GetValue(1, i) == null)
                    theArray[i - 1] = 0;
                else
                    theArray[i - 1] = (double)values.GetValue(1, i);
            }

            return theArray;
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