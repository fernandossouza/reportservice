using System.Collections.Generic;
using System.Threading.Tasks;
using reportservice.Model;

namespace reportservice.Service.Interface
{
    public interface IReportParameterServices
    {
         Task<(Report,string)> GetReportPerProductionOrderId(int productionOrderId,int thingId);
         Task<(Report,string)> GetReportPerRecipeCode(string recipeCode, int thingId, long startDate, long endDate);
    }
}