using System.Threading.Tasks;
using reportservice.Model;

namespace reportservice.Service.Interface
{
    public interface IReportAnalysisService
    {
        Task<(ReportQuality,string)> GetReportQualityPerProductionOrderId(int productionOrderId);
        Task<(ReportQuality,string)> GetReportQualityPerRecipeCodeAndDate(long startDate, long endDate,string recipeCode = null);
    }
}