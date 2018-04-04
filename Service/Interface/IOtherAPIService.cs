using System.Collections.Generic;
using System.Threading.Tasks;
using reportservice.Model;
using reportservice.Model.ProductionOrder;

namespace reportservice.Service.Interface
{
    public interface IOtherAPIService
    {
         Task<Report> GetReportAPI (int thingId, long startDate, long endDate);
         Task<List<HistState>> GetHistState(int productionOrderId);
         Task<List<int>> GetHistStateProductionOrderList(string status,long startDate,long endDate);
         Task<ProductionOrder> GetProductionOrderPerId(int productionOrderId);
    }
}