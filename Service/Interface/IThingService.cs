using System.Net;
using System.Threading.Tasks;
using reportservice.Model.Thing;

namespace reportservice.Service.Interface
{
    public interface IThingService
    {
        Task<(Thing, HttpStatusCode)> getThing(int thingId);
    }
}