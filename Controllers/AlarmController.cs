using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reportservice.Model;
using reportservice.Service;
using reportservice.Service.Interface;

namespace reportservice.Controllers{

    [Route("")]
    public class AlarmController : Controller{

        private IManagerAlarmListService alarmService;
        public AlarmController(IManagerAlarmListService alarmService){
            this.alarmService = alarmService;
        }

        [HttpGet("teste")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAlarms([FromQuery] int thingId, [FromQuery] long startDate, [FromQuery] long endDate){       
            Console.WriteLine("Entrou no endpoint");
            Console.WriteLine("");   
            var (alarm, status) = await this.alarmService.getAlarms(thingId,startDate, endDate);
            if(status == HttpStatusCode.OK)                                   
                return Ok(alarm);
            else
                return BadRequest();
        }        
    }
}