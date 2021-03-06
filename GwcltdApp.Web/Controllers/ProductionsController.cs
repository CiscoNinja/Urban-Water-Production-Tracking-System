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

        double m1Total = 0; double m2Total = 0;double m3Total = 0;double m4Total = 0;double m5Total = 0;double m6Total = 0;
        double m7Total = 0;double m8Total = 0;double m9Total = 0;double m10Total = 0;double m11Total = 0;double m12Total = 0;
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

        [Authorize(Roles = "Super")]
        [Route("masterSummary/{listOption:int}/{id}")]
        public HttpResponseMessage GetMasterTable(HttpRequestMessage request, int listOption, string id)
        {
            var allsys = SummaryManager.GetAllSystems();//change underying code to get systems from the logged in user's station
            //List<GraphData> list = new List<GraphData>();
            var graph = new MyDictionary();
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var allProductions = _productionsRepository.GetAll();
                switch (id)
                {
                    case "Treatment Plant Losses(m³m)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 1);
                                var febTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 2);
                                var marchTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 3);
                                var aprilTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 4);
                                var mayTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 5);
                                var juneTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 6);
                                var july7Total = SummaryManager.PlantLoss_metre(item.Code, listOption, 7);
                                var augTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 8);
                                var septTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 9);
                                var octTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 10);
                                var novTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 11);
                                var decTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 12);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Treatment Plant Losses":
                        {
                            foreach (var item in allsys)
                            {
                                //string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);
                                var janTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 1);
                                var febTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 2);
                                var marchTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 3);
                                var aprilTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 4);
                                var mayTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 5);
                                var juneTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 6);
                                var july7Total = SummaryManager.PlantLoss_percent(item.Code, listOption, 7);
                                var augTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 8);
                                var septTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 9);
                                var octTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 10);
                                var novTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 11);
                                var decTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 12);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Daily Average(m³m)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 1);
                                var febTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 2);
                                var marchTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 3);
                                var aprilTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 4);
                                var mayTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 5);
                                var juneTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 6);
                                var july7Total = SummaryManager.getDailyAverageSingle(item.Code, listOption, 7);
                                var augTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 8);
                                var septTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 9);
                                var octTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 10);
                                var novTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 11);
                                var decTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 12);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Treatment Plant Capacity":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getPlantCap(item.Code, listOption, 1, item.Code);
                                var febTotal = SummaryManager.getPlantCap(item.Code, listOption, 2, item.Code);
                                var marchTotal = SummaryManager.getPlantCap(item.Code, listOption, 3, item.Code);
                                var aprilTotal = SummaryManager.getPlantCap(item.Code, listOption, 4, item.Code);
                                var mayTotal = SummaryManager.getPlantCap(item.Code, listOption, 5, item.Code);
                                var juneTotal = SummaryManager.getPlantCap(item.Code, listOption, 6, item.Code);
                                var july7Total = SummaryManager.getPlantCap(item.Code, listOption, 7, item.Code);
                                var augTotal = SummaryManager.getPlantCap(item.Code, listOption, 8, item.Code);
                                var septTotal = SummaryManager.getPlantCap(item.Code, listOption, 9, item.Code);
                                var octTotal = SummaryManager.getPlantCap(item.Code, listOption, 10, item.Code);
                                var novTotal = SummaryManager.getPlantCap(item.Code, listOption, 11, item.Code);
                                var decTotal = SummaryManager.getPlantCap(item.Code, listOption, 12, item.Code);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


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
                    case "Treated Water(m³)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  1, "Treated Water");
                                var febTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  2, "Treated Water");
                                var marchTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  3, "Treated Water");
                                var aprilTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  4, "Treated Water");
                                var mayTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  5, "Treated Water");
                                var juneTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  6, "Treated Water");
                                var july7Total = SummaryManager.getWaterTableSingle(item.Code, listOption,  7, "Treated Water");
                                var augTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  8, "Treated Water");
                                var septTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  9, "Treated Water");
                                var octTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  10, "Treated Water");
                                var novTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  11, "Treated Water");
                                var decTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  12, "Treated Water");
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Raw Water(m³)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  1, "Raw Water");
                                var febTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  2, "Raw Water");
                                var marchTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  3, "Raw Water");
                                var aprilTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  4, "Raw Water");
                                var mayTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  5, "Raw Water");
                                var juneTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  6, "Raw Water");
                                var july7Total = SummaryManager.getWaterTableSingle(item.Code, listOption,  7, "Raw Water");
                                var augTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  8, "Raw Water");
                                var septTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  9, "Raw Water");
                                var octTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  10, "Raw Water");
                                var novTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  11, "Raw Water");
                                var decTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  12, "Raw Water");
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


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

        [Route("summary/{userstation:int}/{listOption:int}/{id}")]
        public HttpResponseMessage GetTable(HttpRequestMessage request, int userstation, int listOption, string id)
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
                    case "Treatment Plant Losses(m³m)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 1);
                                var febTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 2);
                                var marchTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 3);
                                var aprilTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 4);
                                var mayTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 5);
                                var juneTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 6);
                                var july7Total = SummaryManager.PlantLoss_metre(item.Code, listOption, 7);
                                var augTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 8);
                                var septTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 9);
                                var octTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 10);
                                var novTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 11);
                                var decTotal = SummaryManager.PlantLoss_metre(item.Code, listOption, 12);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal,
                                    july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Treatment Plant Losses":
                        {
                            foreach (var item in allsys)
                            {
                                //string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);
                                var janTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 1);
                                var febTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 2);
                                var marchTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 3);
                                var aprilTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 4);
                                var mayTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 5);
                                var juneTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 6);
                                var july7Total = SummaryManager.PlantLoss_percent(item.Code, listOption, 7);
                                var augTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 8);
                                var septTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 9);
                                var octTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 10);
                                var novTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 11);
                                var decTotal = SummaryManager.PlantLoss_percent(item.Code, listOption, 12);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Daily Average(m³m)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 1);
                                var febTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 2);
                                var marchTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 3);
                                var aprilTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 4);
                                var mayTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 5);
                                var juneTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 6);
                                var july7Total = SummaryManager.getDailyAverageSingle(item.Code, listOption, 7);
                                var augTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 8);
                                var septTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 9);
                                var octTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 10);
                                var novTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 11);
                                var decTotal = SummaryManager.getDailyAverageSingle(item.Code, listOption, 12);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);

                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Treatment Plant Capacity":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getPlantCap(item.Code, listOption, 1, item.Code);
                                var febTotal = SummaryManager.getPlantCap(item.Code, listOption, 2, item.Code);
                                var marchTotal = SummaryManager.getPlantCap(item.Code, listOption, 3, item.Code);
                                var aprilTotal = SummaryManager.getPlantCap(item.Code, listOption, 4, item.Code);
                                var mayTotal = SummaryManager.getPlantCap(item.Code, listOption, 5, item.Code);
                                var juneTotal = SummaryManager.getPlantCap(item.Code, listOption, 6, item.Code);
                                var july7Total = SummaryManager.getPlantCap(item.Code, listOption, 7, item.Code);
                                var augTotal = SummaryManager.getPlantCap(item.Code, listOption, 8, item.Code);
                                var septTotal = SummaryManager.getPlantCap(item.Code, listOption, 9, item.Code);
                                var octTotal = SummaryManager.getPlantCap(item.Code, listOption, 10, item.Code);
                                var novTotal = SummaryManager.getPlantCap(item.Code, listOption, 11, item.Code);
                                var decTotal = SummaryManager.getPlantCap(item.Code, listOption, 12, item.Code);
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);

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
                    case "Treated Water(m³)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  1, "Treated Water");
                                var febTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  2, "Treated Water");
                                var marchTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  3, "Treated Water");
                                var aprilTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  4, "Treated Water");
                                var mayTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  5, "Treated Water");
                                var juneTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  6, "Treated Water");
                                var july7Total = SummaryManager.getWaterTableSingle(item.Code, listOption,  7, "Treated Water");
                                var augTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  8, "Treated Water");
                                var septTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  9, "Treated Water");
                                var octTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  10, "Treated Water");
                                var novTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  11, "Treated Water");
                                var decTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  12, "Treated Water");
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


                            response = request.CreateResponse<MyDictionary>(HttpStatusCode.OK, graph);
                            break;
                        }
                    case "Raw Water(m³)":
                        {
                            foreach (var item in allsys)
                            {
                                var janTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  1, "Raw Water");
                                var febTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  2, "Raw Water");
                                var marchTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  3, "Raw Water");
                                var aprilTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  4, "Raw Water");
                                var mayTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  5, "Raw Water");
                                var juneTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  6, "Raw Water");
                                var july7Total = SummaryManager.getWaterTableSingle(item.Code, listOption,  7, "Raw Water");
                                var augTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  8, "Raw Water");
                                var septTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  9, "Raw Water");
                                var octTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  10, "Raw Water");
                                var novTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  11, "Raw Water");
                                var decTotal = SummaryManager.getWaterTableSingle(item.Code, listOption,  12, "Raw Water");
                                graph.Add(item.Name, janTotal, febTotal, marchTotal, aprilTotal, mayTotal, juneTotal, july7Total, augTotal, septTotal, octTotal, novTotal, decTotal);
                                m1Total += janTotal; m2Total += febTotal; m3Total += marchTotal; m4Total += aprilTotal; m5Total += mayTotal;
                                m6Total += juneTotal; m7Total += july7Total; m8Total += augTotal; m9Total += septTotal; m10Total += octTotal;
                                m11Total += novTotal; m12Total += decTotal;
                            }
                            graph.Add("TOTAL", m1Total, m2Total, m3Total, m4Total, m5Total, m6Total, m7Total, m8Total, m9Total, m10Total, m11Total, m12Total);


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

        [Route("charts/{userstation:int}/{listOption:int}/{id}")]
        public HttpResponseMessage GetChart(HttpRequestMessage request, int listOption, int userstation, string id)
        {
            var allsys = SummaryManager.GetAllUserSystems(userstation);
            List<Dictionary<string, string>> returnList = new List<Dictionary<string, string>>();

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                switch (id)
                {
                    case "Treated Water":
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                Dictionary<string, string> graph_dictionary = new Dictionary<string, string>();
                                DateTime monthvalue = new DateTime(2016, i, 1);
                                string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);

                                graph_dictionary["month"] = thismonth;

                                foreach (var syscode in allsys)
                                {
                                    graph_dictionary[syscode.Code] = SummaryManager.getWaterTableSingle(syscode.Code, listOption, i, "Treated Water").ToString();
                                }

                                returnList.Add(graph_dictionary);
                            }

                            response = request.CreateResponse(HttpStatusCode.OK, returnList);
                            break;
                        }
                    case "Plant Loss":
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                Dictionary<string, string> graph_dictionary = new Dictionary<string, string>();
                                DateTime monthvalue = new DateTime(2016, i, 1);
                                string thismonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthvalue.Month);

                                graph_dictionary["month"] = thismonth;

                                foreach (var syscode in allsys)
                                {
                                    graph_dictionary[syscode.Code] = SummaryManager.PlantLoss_percent(syscode.Code, listOption, i).ToString();
                                }

                                returnList.Add(graph_dictionary);
                            }

                            response = request.CreateResponse(HttpStatusCode.OK, returnList);
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
                if (filter1.HasValue && filter2.HasValue && string.IsNullOrEmpty(filter))
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