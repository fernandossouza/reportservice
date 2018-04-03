using System.Collections.Generic;
using System.Threading.Tasks;
using reportservice.Model;

namespace reportservice.Service.Interface
{
    public interface IOtherAPIService
    {
         Task<Report> GetReportAPI (int thingId, long startDate, long endDate);
         Task<List<HistState>> GetHistState(int productionOrderId);
    }
}