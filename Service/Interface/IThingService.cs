using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using reportservice.Model.Thing;

namespace reportservice.Service.Interface
{
    public interface IThingService
    {
        Task<(List<Thing>, HttpStatusCode)> getThing(int? startat, int? quantity,
            string fieldFilter=null, string fieldValue = null,
            string orderField=null, string order=null);
    }
}