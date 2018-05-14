using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using reportservice.Service.Interface;
using reportservice.Model;
using reportservice.Model.Quality;
using reportservice.Model.ProductionOrder;

namespace reportservice.Service
{
    public class ReportAnalysisService : IReportAnalysisService
    {
        private readonly IOtherAPIService _otherAPIService;
        private List<ProductionOrder> _productionOrderList;

        public ReportAnalysisService(IOtherAPIService otherAPIService)
        {
            _otherAPIService = otherAPIService;
            _productionOrderList = new List<ProductionOrder>();
        }

        public async Task<(ReportQuality,string)> GetReportQualityPerProductionOrderId(int productionOrderId)
        {
            var productionOrderQuality = await _otherAPIService.GetQualityPerProductionOrderId(productionOrderId);

            if(productionOrderQuality == null)
                return(null,"Not found");
            
            List<ProductionOrderQuality> productionOrderQualityList = new List<ProductionOrderQuality>();

            productionOrderQualityList.Add(productionOrderQuality);
            var reportRetun = await ConvertAnalisysToReportAnalisys(productionOrderQualityList);

            return (reportRetun,string.Empty);
        }

        public async Task<(ReportQuality,string)> GetReportQualityPerRecipeCodeAndDate(long startDate, long endDate,string recipeCode = null)
        {
            List<ProductionOrderQuality> productionOrderQualityList =null;
            if(recipeCode != null)
                productionOrderQualityList = await _otherAPIService.GetQualityPerRecipeCode(recipeCode,startDate,endDate);
            else
                productionOrderQualityList = await _otherAPIService.GetQualityPerDate(startDate,endDate);

            if(productionOrderQualityList == null || productionOrderQualityList.Count == 0)                
            return(null,"Not found");
            
            var reportRetun = await ConvertAnalisysToReportAnalisys(productionOrderQualityList);

            return (reportRetun,string.Empty);
        }
       
        private async Task<ReportQuality> ConvertAnalisysToReportAnalisys(List<ProductionOrderQuality> productionQualityList)
        {
            ReportQuality report = new ReportQuality();
            report.Report= new List<ReportAnalysis>();

            foreach(var productionOrderQuality in productionQualityList)
            {
                foreach(var analisys in productionOrderQuality.Analysis)
                {
                    foreach(var comp in analisys.comp)
                    {
                        try{
                        ReportAnalysis reportAnalisys = new ReportAnalysis();

                        reportAnalisys.numberAnalysis = analisys.number;
                        reportAnalisys.date = analisys.datetime;
                        reportAnalisys.op = productionOrderQuality.productionOrderNumber;
                        reportAnalisys.status = analisys.status;
                        
                        var productionOrderGet = await ProductionOrderReport(productionOrderQuality.productionOrderId);
                        var  phase = productionOrderGet.recipe.phases.FirstOrDefault();
                        var productRecipe = phase.phaseProducts.Where(z=>z.product.productId == comp.productId).FirstOrDefault();
                        reportAnalisys.productId = comp.productId;
                        reportAnalisys.recipeMin = productRecipe.minValue;
                        reportAnalisys.recipeMax = productRecipe.maxValue;
                        reportAnalisys.productName = productRecipe.product.productName;
                        reportAnalisys.resultAnalysis = comp.value;

                        var correction = analisys.messages.Where(x=>x.key==reportAnalisys.productName).FirstOrDefault();
                        if(correction == null)
                            reportAnalisys.correction = "0";
                        else
                            reportAnalisys.correction = correction.value;

                        reportAnalisys.userName = analisys.userName;
                        

                        report.Report.Add(reportAnalisys);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            return report;
        }

        private async Task<ProductionOrder> ProductionOrderReport(int productionOrderId)
        {
            ProductionOrder productionOrder = null;

            productionOrder = _productionOrderList.Where(x=>x.productionOrderId == productionOrderId).FirstOrDefault();

            if(productionOrder == null)
            {
                productionOrder = await _otherAPIService.GetProductionOrderPerId(productionOrderId);

                if(productionOrder != null)
                    _productionOrderList.Add(productionOrder);
            }
            return productionOrder;
        }
        
    }
}