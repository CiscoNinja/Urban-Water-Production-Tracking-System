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
        private readonly IEntityBaseRepository<WSystem> _wsystemRepository;
        private readonly IEntityBaseRepository<SystemProduction> _systemproductionRepository;

        public struct EccellCells
        {
            public string option { get; set; }
            public string OptionType { get; set; }
            public string[] cellList { get; set; }
        }

        public class MyDictionary : List<EccellCells>
        {
            public void Add(string key, string option, string[] xcell)
            {
                EccellCells val = new EccellCells();
                val.OptionType = key;
                val.option = option;
                val.cellList = xcell;
                this.Add(val);
            }
        }

        public HourlyProductionsController(IEntityBaseRepository<HourlyProduction> hrlyproductionsRepository,
            IEntityBaseRepository<Production> newproductionsRepository,
            IEntityBaseRepository<WSystem> wsystemRepository,
            IEntityBaseRepository<SystemProduction> systemproductionRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _wsystemRepository = wsystemRepository;
            _systemproductionRepository = systemproductionRepository;
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
                    if (_hrlyproductionsRepository.ProductionExists(hrlyproduction.DayToRecord, hrlyproduction.WSystemId,
                        hrlyproduction.OptionId, hrlyproduction.OptionTypeId, hrlyproduction.GwclStationId, hrlyproduction.DailyActual))
                    {
                        ModelState.AddModelError("Invalid production", "Record already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        HourlyProduction newhrlyProduction = new HourlyProduction();
                        newhrlyProduction.UpdateHrlyProduction(hrlyproduction);


                        _hrlyproductionsRepository.Add(newhrlyProduction);

                        _unitOfWork.Commit();

                        addProductioToSystem(newhrlyProduction, hrlyproduction.WSystemId);

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
                }
                return response;
            });
        }

        private void addProductioToSystem(HourlyProduction thishourprod, int systemId)
        {
            var gwclsystem = _wsystemRepository.GetSingle(systemId);
            if (gwclsystem == null)
                throw new ApplicationException("system doesn't exist.");

            var systemProduction = new SystemProduction()
            {
                ProductionId = thishourprod.ID,
                WSystemId = gwclsystem.ID
            };
            _systemproductionRepository.Add(systemProduction);
        }

        [HttpPost]
        [Route("importExcel")]
        public HttpResponseMessage ImportFromExcel(HttpRequestMessage request, ExcelViewModel hrlyproduction)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            var graph = new MyDictionary();

            graph.Add("Raw Water 1", "Raw Water", new string[] { "E6", "AI6", "E19", "AG19", "E32", "AI32", "E45",
                    "AH45", "E58", "AI58", "E71", "AH71", "E84", "AI84", "E97", "AI97", "E110", "AH110", "E123", "AI123", "E136", "AH136", "E149", "AI149"});
            graph.Add("Raw Water 2", "Raw Water", new string[] { "E7", "AI7", "E20", "AG20", "E33", "AI33", "E46",
                    "AH46", "E59", "AI59", "E72", "AH72", "E85", "AI85", "E98", "AI98", "E111", "AH111", "E124", "AI124", "E137", "AH137", "E150", "AI150"});
            graph.Add("Raw Water 3", "Raw Water", new string[] { "E8", "AI8", "E21", "AG21", "E34", "AI34", "E47",
                    "AH47", "E60", "AI60", "E73", "AH73", "E86", "AI86", "E99", "AI99", "E112", "AH112", "E125", "AI125", "E138", "AH138", "E151", "AI151"});
            graph.Add("Raw Water 4", "Raw Water", new string[] { "E9", "AI9", "E22", "AG22", "E35", "AI35", "E48",
                    "AH48", "E61", "AI61", "E74", "AH74", "E87", "AI87", "E100", "AI100", "E113", "AH113", "E126", "AI126", "E139", "AH139", "E152", "AI152"});

            graph.Add("Treated Water 1", "Treated Water", new string[] { "E12", "AI12", "E25", "AG25", "E38", "AI38",
                    "E51", "AH51", "E64", "AI64", "E77", "AH77","E90", "AI90", "E103", "AI103", "E116", "AH116", "E129", "AI129", "E142", "AH142", "E155", "AI155" });
            graph.Add("Treated Water 2", "Treated Water", new string[] { "E13", "AI13", "E26", "AG26", "E39", "AI39",
                    "E52", "AH52", "E65", "AI65", "E78", "AH78","E91", "AI91", "E104", "AI104", "E117", "AH117", "E130", "AI130", "E143", "AH143", "E156", "AI156" });
            graph.Add("Treated Water 3", "Treated Water", new string[] { "E14", "AI14", "E27", "AG27", "E40", "AI40",
                    "E53", "AH53", "E66", "AI66", "E79", "AH79","E92", "AI92", "E105", "AI105", "E118", "AH118", "E131", "AI131", "E144", "AH144", "E157", "AI157" });
            graph.Add("Treated Water 4", "Treated Water", new string[] { "E15", "AI15", "E28", "AG28", "E41", "AI41",
                    "E54", "AH54", "E67", "AI67", "E80", "AH80","E93", "AI93", "E106", "AI106", "E119", "AH119", "E132", "AI132", "E145", "AH145", "E158", "AI158" });
            //int rpositn = 0;

            string typeName = null; //water type eg. Raw Water 1
            string optionName = null; 
            object[,] waterRecords = null; // daily record valuea on excel sheet
            string[] tableCells = null; // cells on excel sheet
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;

                ProductionViewModel newViewModel = new ProductionViewModel();
                HourlyProduction newhrlyProduction = new HourlyProduction();
                Production newproduction = new Production();
                //string fullpath = Path.GetFullPath(fileName);
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@hrlyproduction.filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                var scode = SummaryManager.GetSystemCode(hrlyproduction.WSystemId);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(scode.Substring(scode.LastIndexOf('-') + 1));

                //var cellLists = SummaryManager.GetCells();

                foreach (var list in graph)
                {
                    optionName = list.option;
                    typeName = list.OptionType;
                    tableCells = list.cellList;

                    int cmonth = 0;
                    int rday;
                    int tday;
                    double rTFPD = 0;
                    double tTFPD = 0;

                    if (optionName.Equals("Raw Water"))
                    {
                        for (int valPos = 0; valPos < 24; valPos += 2)
                        {
                            rday = 1;
                            tday = 1;
                            cmonth++;
                            int nextpost = valPos + 1;
                            int numOfDays = DateTime.DaysInMonth(hrlyproduction.DayToRecord.Year, cmonth);
                            Excel.Range rRange = xlWorkSheet.Range[xlWorkSheet.Range[tableCells[valPos]], xlWorkSheet.Range[tableCells[nextpost]]];
                            //double rangeVal = rRange.Cells.Value2;
                            waterRecords = rRange.Value2;

                            for (int i = 1; i <= numOfDays; i++)
                            {
                                var thisVal = waterRecords.GetValue(1,i);
                                if (thisVal == null)
                                {
                                    //!double.IsNaN(Convert.ToDouble(thisVal))
                                    //rTFPD += 0;
                                    //newViewModel.Comment = "No record found on excel";
                                    //newViewModel.DailyActual = 0;
                                }
                                else
                                {
                                    rTFPD += Convert.ToDouble(thisVal);
                                    newViewModel.Comment = hrlyproduction.Comment;
                                    newViewModel.DailyActual = Convert.ToDouble(thisVal);

                                newViewModel.DateCreated = Convert.ToDateTime((cmonth + "/" + rday + "/" + hrlyproduction.DayToRecord.Year + " 01:00 am").ToString());
                                newViewModel.DayToRecord = Convert.ToDateTime((cmonth + "/" + rday + "/" + hrlyproduction.DayToRecord.Year + " 01:00 am").ToString());
                                newViewModel.FRPH = hrlyproduction.FRPH;
                                newViewModel.FRPS = hrlyproduction.FRPS;
                                newViewModel.GwclStation = hrlyproduction.GwclStation;
                                newViewModel.GwclStationId = hrlyproduction.GwclStationId;
                                newViewModel.LOG = hrlyproduction.LOG;
                                newViewModel.NTFPD = hrlyproduction.NTFPD;
                                newViewModel.Option = "Raw Water";
                                newViewModel.OptionId = SummaryManager.GetOptionIdByName("Raw Water");
                                newViewModel.OptionType = typeName;
                                newViewModel.OptionTypeId = SummaryManager.GetOptionTypeIdByName(typeName);
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

                                addProductioToSystem(newhrlyProduction, hrlyproduction.WSystemId);

                                _unitOfWork.Commit();
                                }
                                rday++;
                            }
                        }
                    }
                    else if (optionName.Equals("Treated Water"))
                    {
                        for (int valPos = 0; valPos < 24; valPos += 2)
                        {
                            tday = 1;
                            cmonth++;
                            int nextpost = valPos + 1;
                            int numOfDays = DateTime.DaysInMonth(hrlyproduction.DayToRecord.Year, cmonth);
                            Excel.Range rRange = xlWorkSheet.Range[xlWorkSheet.Range[tableCells[valPos]], xlWorkSheet.Range[tableCells[nextpost]]];
                            //double rangeVal = rRange.Cells.Value2;
                            waterRecords = rRange.Value2;
                            for (int i = 1; i<=numOfDays; i++)
                            {
                                var thisVal = waterRecords.GetValue(1, i);
                                if (thisVal == null)
                                {
                                    //rTFPD += 0;
                                    //newViewModel.Comment = "No record found on excel";
                                    //newViewModel.DailyActual = 0;
                                }
                                else
                                {
                                    tTFPD += Convert.ToDouble(thisVal);
                                    newViewModel.Comment = hrlyproduction.Comment;
                                    newViewModel.DailyActual = Convert.ToDouble(thisVal);
                                    newViewModel.DateCreated = Convert.ToDateTime((cmonth + "/" + tday + "/" + hrlyproduction.DayToRecord.Year + " 01:00 am").ToString());
                                    newViewModel.DayToRecord = Convert.ToDateTime((cmonth + "/" + tday + "/" + hrlyproduction.DayToRecord.Year + " 01:00 am").ToString());
                                    newViewModel.FRPH = hrlyproduction.FRPH;
                                    newViewModel.FRPS = hrlyproduction.FRPS;
                                    newViewModel.GwclStation = hrlyproduction.GwclStation;
                                    newViewModel.GwclStationId = hrlyproduction.GwclStationId;
                                    newViewModel.LOG = hrlyproduction.LOG;
                                    newViewModel.NTFPD = hrlyproduction.NTFPD;
                                    newViewModel.Option = "Treated Water";
                                    newViewModel.OptionId = SummaryManager.GetOptionIdByName("Treated Water");
                                    newViewModel.OptionType = typeName;
                                    newViewModel.OptionTypeId = SummaryManager.GetOptionTypeIdByName(typeName);
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

                                    addProductioToSystem(newhrlyProduction, hrlyproduction.WSystemId);

                                    _unitOfWork.Commit();
                                }
                                tday++;
                            }
                        }
                    }
                    else
                    {

                    }
                }

                xlWorkBook.Close(false, null, null);
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

        #region fileupload
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
        #endregion fileupload
    }
}