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

        [HttpGet("api/alarmreport")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAlarms([FromQuery] int thingId, [FromQuery] long startDate, [FromQuery] long endDate, [FromQuery] int opId){       
            Console.WriteLine("Entrou no endpoint");
            Console.WriteLine("");   
            var (relatorio, status) = await this.alarmService.defineGet(opId, thingId,startDate, endDate);
            Console.WriteLine(relatorio.ToString());
            if(status == HttpStatusCode.OK)                                   
                return Ok(relatorio);
            else
                return BadRequest();
        }        
    }
}