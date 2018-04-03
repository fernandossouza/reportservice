using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using reportservice.Service.Interface;
using reportservice.Model;

namespace reportservice.Service
{
    public class ReportOpService : IReportOpService
    {
        private readonly IOtherAPIService _otherAPIService;

        public ReportOpService(IOtherAPIService otherAPIService)
        {
            _otherAPIService = otherAPIService;
        }

        public async Task<(Report,string)> GetReport(int productionOrderId,int thingId)
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


    }
}