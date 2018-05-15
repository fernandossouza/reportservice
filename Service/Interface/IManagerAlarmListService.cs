using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using reportservice.Model;

namespace reportservice.Service.Interface{

    public interface IManagerAlarmListService{
            Task<(List<AlarmFront>, HttpStatusCode)> defineGet(int opId, int thingId, long startDate, long endDate);
    }
}