
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using reportservice.Model;
namespace reportservice.Service.Interface{

        public interface IAlarmService{
                
                Task<(List<Alarm>, HttpStatusCode)> getAlarm(int thingId, long startDate, long endDate);
        }
}