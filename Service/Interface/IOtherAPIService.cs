using System.Collections.Generic;
using System.Threading.Tasks;
using reportservice.Model;
using reportservice.Model.ProductionOrder;
using reportservice.Model.Quality;

namespace reportservice.Service.Interface
{
    public interface IOtherAPIService
    {
         Task<Report> GetReportAPI (int thingId, long startDate, long endDate);
         Task<List<HistState>> GetHistState(int productionOrderId);
         Task<List<int>> GetHistStateProductionOrderList(string status,long startDate,long endDate);
         Task<ProductionOrder> GetProductionOrderPerId(int productionOrderId);
         Task<ProductionOrderQuality> GetQualityPerProductionOrderId(int productionOrderId);
         Task<List<ProductionOrderQuality>> GetQualityPerDate(long startDate, long endDate);
         Task<List<ProductionOrderQuality>> GetQualityPerRecipeCode(string recipeCode,long startDate, long endDate);
    }
}