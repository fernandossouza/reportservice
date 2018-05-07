using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reportservice.Model;
using reportservice.Service;
using reportservice.Service.Interface;

namespace reportservice.Controllers{

    [Route("")]
    public class AlarmController : Controller{

        private IAlarmService alarmService;
        public AlarmController(IAlarmService alarmService){
            this.alarmService = alarmService;
        }

        [HttpGet("teste")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAlarms(){                                              
            return Ok(await this.alarmService.getAlarm(13,000000000000000000, 999999999999999999));
        }        
    }
}