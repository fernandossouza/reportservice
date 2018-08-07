
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using reportservice.Service.Interface;

namespace reportservice.Controllers{
    [Route ("api/[controller]")]
    public class GenealogyController : Controller{

        private IGenealogyService genealogyService;

        public GenealogyController(IGenealogyService genealogyService){
            this.genealogyService = genealogyService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetGenealogyOp([FromQuery] int startDate, [FromQuery] long endDate){       
            
            var (orders, status) = await this.genealogyService.getGenealogyOpAsync(startDate, endDate);
            //var (relatorio, status) = await this.alarmService.defineGet(opId, thingId,startDate, endDate);                    
            return Ok(orders);            
        }  
    }
}        