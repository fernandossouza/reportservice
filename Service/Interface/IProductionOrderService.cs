using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using reportservice.Model.ProductionOrder;

namespace reportservice.Service.Interface
{
    public interface IProductionOrderService{
        Task<(List<ProductionOrder>, HttpStatusCode)> getProductionOrder(int? startat, int? quantity,
            string fieldFilter=null, string fieldValue=null,string orderField=null, string order=null);
    }
}