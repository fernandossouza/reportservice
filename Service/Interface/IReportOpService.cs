using System.Collections.Generic;
using System.Threading.Tasks;
using reportservice.Model;

namespace reportservice.Service.Interface
{
    public interface IReportOpService
    {
         Task<(Report,string)> GetReport(int productionOrderId,int thingId);
    }
}