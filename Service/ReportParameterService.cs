using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using reportservice.Service.Interface;
using reportservice.Model;

namespace reportservice.Service
{
    public class ReportParameterService : IReportParameterServices
    {
        private readonly IOtherAPIService _otherAPIService;

        public ReportParameterService(IOtherAPIService otherAPIService)
        {
            _otherAPIService = otherAPIService;
        }

        public async Task<(Report,string)> GetReportPerProductionOrderId(int productionOrderId,int thingId)
        {
            long startDate = 0;
            long endDate = 0;
            //Verifica a data inicio e data fim da ordem de produção
            var histState = await _otherAPIService.GetHistState(productionOrderId);

            if(histState == null)
                return (null,"Historian State not found");

            var histStateStartDate = histState.Where(x=>x.state == "active").FirstOrDefault();

            var histStateEndDate = histState.Where(x=>x.state == "ended").FirstOrDefault();

            if(histStateStartDate == null)
                return (null,"Start date null");

            if(histStateEndDate == null)
                endDate = 999999999999999999;
            else
                endDate = histStateEndDate.date;

            startDate = histStateStartDate.date;

            var report = await _otherAPIService.GetReportAPI(thingId,startDate,endDate);

            if(report == null)
                return (null,"Not found report");

            return (report,string.Empty);
        }

        private async Task<(Report,string)> AddReportPeriod(Report reportReturn,int productionOrderId,int thingId)
        {
            var (report,stringErro) = await GetReportPerProductionOrderId(productionOrderId,thingId);

            if(report == null)
                return (null,stringErro);

            foreach(var tag in report.tags)
            {
                var newTag = reportReturn.tags.Where(x=>x.group== tag.group && x.name == tag.name).FirstOrDefault();

                if(newTag == null)
                    reportReturn.tags.Add(tag);
                else
                {
                    newTag.timestamp.AddRange(tag.timestamp);
                    newTag.value.AddRange(tag.value);
                }
            }
            return (reportReturn,string.Empty);
        }

        public async Task<(Report,string)> GetReportPerRecipeCode(string recipeCode, int thingId, long startDate, long endDate)
        {
            Report reportReturn = new Report();
            reportReturn.tags = new List<Tag>();

            if(endDate <= 0)
                endDate = 999999999999999999;

            var productionOrderIds = await _otherAPIService.GetHistStateProductionOrderList("active",startDate,endDate);

            if(productionOrderIds.Count()==0)
                return (null,"Not found ProductionOrders");

            foreach(var productionOrderId in productionOrderIds)
            {
                var productionOrder = await _otherAPIService.GetProductionOrderPerId(productionOrderId);

                if(productionOrder.recipe.recipeCode.ToLower() == recipeCode.ToLower())
                {
                    var (report,stringErro) = await AddReportPeriod(reportReturn,productionOrder.productionOrderId,thingId);

                    if(report == null)
                        return (null,stringErro);
                    
                    reportReturn = report;
                }
            }

            return (reportReturn,string.Empty);

        }


    }
}